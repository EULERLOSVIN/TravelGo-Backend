using Application.DTOs.Dashboard;
using Application.Interfaces.Dashboard;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardSummaryAsync()
    {
        var now = DateTime.Now;
        var todayStart = now.Date;
        var tomorrowStart = todayStart.AddDays(1);
        var yesterdayStart = todayStart.AddDays(-1);
        var monthStart = new DateTime(now.Year, now.Month, 1);
        
        var todayDate = DateOnly.FromDateTime(todayStart);
        var yesterdayDate = DateOnly.FromDateTime(yesterdayStart);
        var monthStartDate = DateOnly.FromDateTime(monthStart);

        // 1. Consolidated Core Metrics (All scalars in 1 execution if EF allows it, else split efficiently)
        // Income Today & Yesterday
        var todayIncome = await _context.Billings.AsNoTracking()
            .Where(b => b.BillingDate >= todayStart && b.BillingDate < tomorrowStart)
            .SumAsync(b => b.TotalAmount);
        
        var yesterdayIncome = await _context.Billings.AsNoTracking()
            .Where(b => b.BillingDate >= yesterdayStart && b.BillingDate < todayStart)
            .SumAsync(b => b.TotalAmount);

        // Tickets Today & Yesterday
        var todayTickets = await _context.TravelTickets.AsNoTracking()
            .Where(tt => tt.TravelDate == todayDate)
            .CountAsync();
            
        var yesterdayTickets = await _context.TravelTickets.AsNoTracking()
            .Where(tt => tt.TravelDate == yesterdayDate)
            .CountAsync();

        // Status Counts
        var inRouteCount = await _context.Trips.AsNoTracking()
            .CountAsync(t => t.IdStateTrip == 1);
            
        var inQueueCount = await _context.AssignQueues.AsNoTracking()
            .CountAsync();

        // 2. Optimized Occupancy (Projections only)
        var occupancyData = await _context.Trips.AsNoTracking()
            .Where(t => t.IdStateTrip == 1)
            .Select(t => new {
                Capacity = t.IdVehicleNavigation.DetailVehicles.Select(d => (int?)d.SeatNumber).FirstOrDefault() ?? 1,
                Sold = _context.TravelTickets.Count(tt => tt.IdVehicle == t.IdVehicle && tt.TravelDate == todayDate)
            })
            .ToListAsync();

        int avgOccupancy = occupancyData.Any() 
            ? (int)occupancyData.Average(x => (float)x.Sold / x.Capacity * 100) 
            : 0;

        // 3. Top Routes (Month) - Optimized grouping
        var topRoutes = await _context.TravelTickets.AsNoTracking()
            .Where(tt => tt.TravelDate >= monthStartDate)
            .GroupBy(tt => tt.IdTravelRouteNavigation.NameRoute)
            .Select(g => new RouteProfitabilityDto
            {
                RouteName = g.Key,
                TotalIncome = g.Sum(tt => tt.IdBillingNavigation.TotalAmount)
            })
            .OrderByDescending(x => x.TotalIncome)
            .Take(5)
            .ToListAsync();

        var totalMonthIncome = topRoutes.Sum(r => r.TotalIncome);
        foreach (var r in topRoutes)
        {
            r.PercentageOfTotal = (totalMonthIncome == 0) ? 0 : (int)(r.TotalIncome / totalMonthIncome * 100);
        }

        // 4. Hourly Demand
        var hourlyDemand = await _context.Billings.AsNoTracking()
            .Where(b => b.BillingDate >= todayStart && b.BillingDate < tomorrowStart)
            .GroupBy(b => b.BillingDate.Value.Hour)
            .Select(g => new HourlyDemandDto
            {
                Hour = g.Key + ":00",
                TicketsSold = g.Count()
            })
            .OrderByDescending(x => x.TicketsSold)
            .ToListAsync();

        // 5. Upcoming Departures (Deep Projection)
        var upcoming = await _context.AssignQueues.AsNoTracking()
            .OrderBy(aq => aq.IdAssignQueue)
            .Take(10)
            .Select(aq => new {
                aq.IdVehicle,
                RouteName = aq.IdTravelRouteNavigation.NameRoute,
                Driver = aq.IdVehicleNavigation.IdPersonNavigation != null 
                    ? aq.IdVehicleNavigation.IdPersonNavigation.FirstName + " " + aq.IdVehicleNavigation.IdPersonNavigation.LastName 
                    : "S/N",
                aq.IdVehicleNavigation.PlateNumber,
                Capacity = aq.IdVehicleNavigation.DetailVehicles.Select(d => (int?)d.SeatNumber).FirstOrDefault() ?? 1,
                Sold = _context.TravelTickets.Count(tt => tt.IdVehicle == aq.IdVehicle && tt.TravelDate == todayDate),
                DepartureTime = aq.IdTravelRouteNavigation.DepartureTimes.OrderBy(dt => dt.Hour).Select(dt => (TimeOnly?)dt.Hour).FirstOrDefault()
            })
            .ToListAsync();

        var upcomingList = upcoming.Select(x => {
            var perc = (int)((float)x.Sold / x.Capacity * 100);
            return new UpcomingDepartureDto {
                Hour = x.DepartureTime?.ToString("HH:mm") ?? "--:--",
                RouteName = x.RouteName,
                DriverName = x.Driver,
                PlateNumber = x.PlateNumber,
                OccupancyPercentage = perc,
                Status = perc >= 80 ? "Listo" : perc >= 40 ? "Cargando" : "Baja ocupación"
            };
        }).ToList();

        // 6. Active Alerts (Combined and projected)
        var limit60Value = todayDate.AddDays(60);
        var alertSoat = await _context.DocumentVehicles.AsNoTracking()
            .Where(dv => dv.SoatExpirationDate >= todayDate && dv.SoatExpirationDate <= limit60Value)
            .Select(dv => new ActiveAlertDto {
                Type = "SOAT",
                Description = "SOAT Unidad " + dv.IdVehicleNavigation.PlateNumber + " vence pronto.",
                Severity = (dv.SoatExpirationDate <= todayDate.AddDays(15)) ? "High" : "Medium"
            }).ToListAsync();

        var alertLicense = await _context.DocumentDrivers.AsNoTracking()
            .Where(dd => dd.LicenseExpirationDate >= todayDate && dd.LicenseExpirationDate <= limit60Value)
            .Select(dd => new ActiveAlertDto {
                Type = "Licencia",
                Description = "Licencia de " + dd.IdPersonNavigation.FirstName + " vence pronto.",
                Severity = (dd.LicenseExpirationDate <= todayDate.AddDays(15)) ? "High" : "Medium"
            }).ToListAsync();

        // 7. Recent Activity (Minimal projection)
        var recentSales = await _context.TravelTickets.AsNoTracking()
            .OrderByDescending(tt => tt.IdBilling)
            .Take(4)
            .Select(tt => new RecentActivityDto {
                Description = "Venta: " + tt.IdTravelRouteNavigation.NameRoute,
                Type = "Sale",
                Date = tt.IdBillingNavigation.BillingDate ?? now
            }).ToListAsync();

        var recentQueue = await _context.AssignQueues.AsNoTracking()
            .OrderByDescending(aq => aq.IdAssignQueue)
            .Take(4)
            .Select(aq => new RecentActivityDto {
                Description = "Unidad " + aq.IdVehicleNavigation.PlateNumber + " en fila",
                Type = "User",
                Date = now.AddMinutes(-10)
            }).ToListAsync();

        var allActivity = recentSales.Concat(recentQueue).OrderByDescending(a => a.Date).ToList();
        foreach (var act in allActivity) {
            var diff = now - act.Date;
            if (diff.TotalMinutes < 60) act.TimeAgo = "Hace " + (int)diff.TotalMinutes + "m";
            else if (diff.TotalHours < 24) act.TimeAgo = "Hace " + (int)diff.TotalHours + "h";
            else act.TimeAgo = "Hace " + (int)diff.TotalDays + "d";
        }

        // 8. Sales Channel Grouping
        var salesStats = await _context.Billings.AsNoTracking()
            .Where(b => b.BillingDate >= todayStart && b.BillingDate < tomorrowStart)
            .GroupBy(b => string.IsNullOrEmpty(b.OperationPayCode))
            .Select(g => new { IsTerminal = g.Key, Count = g.Count() })
            .ToListAsync();

        int tCount = salesStats.FirstOrDefault(s => s.IsTerminal)?.Count ?? 0;
        int wCount = salesStats.FirstOrDefault(s => !s.IsTerminal)?.Count ?? 0;
        int total = tCount + wCount;

        return new DashboardDto {
            DailyMetrics = new DailyMetricsDto {
                TodayIncome = todayIncome, YesterdayIncome = yesterdayIncome, IncomeTrendPercentage = (yesterdayIncome == 0) ? 0 : (int)((todayIncome - yesterdayIncome)/yesterdayIncome*100),
                TodayTicketsSold = todayTickets, YesterdayTicketsSold = yesterdayTickets, TicketsTrendPercentage = (yesterdayTickets == 0) ? 0 : (int)((float)(todayTickets - yesterdayTickets)/yesterdayTickets*100),
                OccupancyRate = avgOccupancy, TotalVehiclesInRoute = inRouteCount, TotalVehiclesInQueue = inQueueCount
            },
            TopRoutes = topRoutes,
            HourlyDemand = hourlyDemand,
            UpcomingDepartures = upcomingList,
            ActiveAlerts = alertSoat.Concat(alertLicense).OrderBy(a => a.Severity == "High" ? 0 : 1).ToList(),
            RecentActivity = allActivity,
            SalesByChannel = new List<SalesChannelDto> {
                new SalesChannelDto { ChannelName = "Terminal", Count = tCount, Percentage = total == 0 ? 0 : (int)((float)tCount/total*100) },
                new SalesChannelDto { ChannelName = "Web", Count = wCount, Percentage = total == 0 ? 0 : (int)((float)wCount/total*100) }
            }
        };
    }
}
