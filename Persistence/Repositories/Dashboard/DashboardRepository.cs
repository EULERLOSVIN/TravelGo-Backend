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
        var today = DateOnly.FromDateTime(DateTime.Now);
        var yesterday = today.AddDays(-1);
        var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // 1. Daily Metrics: Income
        var todayIncome = await _context.Billings
            .AsNoTracking()
            .Where(b => DateOnly.FromDateTime(b.BillingDate.Value) == today)
            .SumAsync(b => b.TotalAmount);

        var yesterdayIncome = await _context.Billings
            .AsNoTracking()
            .Where(b => DateOnly.FromDateTime(b.BillingDate.Value) == yesterday)
            .SumAsync(b => b.TotalAmount);

        int incomeTrend = (yesterdayIncome == 0) ? 0 : (int)((todayIncome - yesterdayIncome) / yesterdayIncome * 100);

        // 2. Daily Metrics: Tickets Sold
        var todayTickets = await _context.TravelTickets
            .AsNoTracking()
            .CountAsync(tt => tt.TravelDate == today);

        var yesterdayTickets = await _context.TravelTickets
            .AsNoTracking()
            .CountAsync(tt => tt.TravelDate == yesterday);

        int ticketsTrend = (yesterdayTickets == 0) ? 0 : (int)((float)(todayTickets - yesterdayTickets) / yesterdayTickets * 100);

        // 3. Vehicles in Route vs Queue
        var inRouteCount = await _context.Trips
            .AsNoTracking()
            .CountAsync(t => t.IdStateTrip == 1); // En Ruta

        var inQueueCount = await _context.AssignQueues
            .AsNoTracking()
            .CountAsync();

        // 4. Occupancy Rate (Simplification for active trips today)
        var activeTripsToday = await _context.Trips
            .AsNoTracking()
            .Include(t => t.IdVehicleNavigation).ThenInclude(v => v.DetailVehicles)
            .Where(t => t.IdStateTrip == 1)
            .ToListAsync();

        int avgOccupancy = 0;
        if (activeTripsToday.Any())
        {
            float totalPerc = 0;
            foreach (var trip in activeTripsToday)
            {
                var soldCount = await _context.TravelTickets
                    .CountAsync(tt => tt.IdVehicle == trip.IdVehicle && tt.TravelDate == today);
                var totalSeats = trip.IdVehicleNavigation.DetailVehicles.FirstOrDefault()?.SeatNumber ?? 1;
                totalPerc += (float)soldCount / totalSeats * 100;
            }
            avgOccupancy = (int)(totalPerc / activeTripsToday.Count);
        }

        // 5. Top Routes (Month)
        var topRoutes = await _context.TravelTickets
            .AsNoTracking()
            .Include(tt => tt.IdTravelRouteNavigation)
            .Include(tt => tt.IdBillingNavigation)
            .Where(tt => tt.TravelDate >= DateOnly.FromDateTime(startOfMonth))
            .GroupBy(tt => tt.IdTravelRouteNavigation.NameRoute)
            .Select(g => new RouteProfitabilityDto
            {
                RouteName = g.Key,
                TotalIncome = g.Sum(x => x.IdBillingNavigation.TotalAmount)
            })
            .OrderByDescending(x => x.TotalIncome)
            .Take(5)
            .ToListAsync();

        var totalMonthIncome = topRoutes.Sum(r => r.TotalIncome);
        foreach (var r in topRoutes)
        {
            r.PercentageOfTotal = (totalMonthIncome == 0) ? 0 : (int)(r.TotalIncome / totalMonthIncome * 100);
        }

        // 6. Hourly Demand (Today)
        var hourlyDemand = await _context.TravelTickets
            .AsNoTracking()
            .Include(tt => tt.IdBillingNavigation)
            .Where(tt => tt.TravelDate == today)
            .ToListAsync();

        var hourlyData = hourlyDemand
            .GroupBy(tt => tt.IdBillingNavigation.BillingDate.Value.Hour)
            .Select(g => new HourlyDemandDto
            {
                Hour = $"{g.Key}:00",
                TicketsSold = g.Count()
            })
            .OrderBy(x => int.Parse(x.Hour.Split(':')[0]))
            .ToList();

        // 7. Upcoming Departures (Top 5 in queue)
        var upcoming = await _context.AssignQueues
            .AsNoTracking()
            .Include(aq => aq.IdVehicleNavigation).ThenInclude(v => v.IdPersonNavigation)
            .Include(aq => aq.IdVehicleNavigation).ThenInclude(v => v.DetailVehicles)
            .Include(aq => aq.IdTravelRouteNavigation).ThenInclude(tr => tr.DepartureTimes)
            .OrderBy(aq => aq.IdAssignQueue)
            .Take(5)
            .ToListAsync();

        var upcomingList = new List<UpcomingDepartureDto>();
        foreach (var aq in upcoming)
        {
            var driver = aq.IdVehicleNavigation.IdPersonNavigation;
            var detail = aq.IdVehicleNavigation.DetailVehicles.FirstOrDefault();
            var soldCount = await _context.TravelTickets.CountAsync(tt => tt.IdVehicle == aq.IdVehicle && tt.TravelDate == today);
            var totalSeats = detail?.SeatNumber ?? 1;
            var perc = (int)((float)soldCount / totalSeats * 100);

            upcomingList.Add(new UpcomingDepartureDto
            {
                Hour = aq.IdTravelRouteNavigation.DepartureTimes.OrderBy(dt => dt.Hour).FirstOrDefault()?.Hour.ToString("HH:mm") ?? "--:--",
                RouteName = aq.IdTravelRouteNavigation.NameRoute,
                DriverName = driver != null ? $"{driver.FirstName} {driver.LastName}" : "S/N",
                PlateNumber = aq.IdVehicleNavigation.PlateNumber,
                OccupancyPercentage = perc,
                Status = perc >= 80 ? "Listo" : perc >= 40 ? "Cargando" : "Baja ocupación"
            });
        }

        // 8. Active Alerts (Security)
        var nextLimit = today.AddMonths(1);
        var alertSoat = await _context.DocumentVehicles
            .Include(dv => dv.IdVehicleNavigation)
            .Where(dv => dv.SoatExpirationDate <= nextLimit && dv.SoatExpirationDate >= today)
            .Select(dv => new ActiveAlertDto
            {
                Type = "SOAT",
                Description = $"SOAT Unidad {dv.IdVehicleNavigation.PlateNumber} vence pronto.",
                Severity = (dv.SoatExpirationDate <= today.AddDays(7)) ? "High" : "Medium"
            }).ToListAsync();

        var alertLicense = await _context.DocumentDrivers
            .Include(dd => dd.IdPersonNavigation)
            .Where(dd => dd.LicenseExpirationDate <= nextLimit && dd.LicenseExpirationDate >= today)
            .Select(dd => new ActiveAlertDto
            {
                Type = "Licencia",
                Description = $"Licencia de {dd.IdPersonNavigation.FirstName} vence pronto.",
                Severity = (dd.LicenseExpirationDate <= today.AddDays(7)) ? "High" : "Medium"
            }).ToListAsync();

        var allAlerts = alertSoat.Concat(alertLicense).ToList();

        // 9. Recent Activity (Latest 10 sales for now)
        var recentSales = await _context.TravelTickets
            .AsNoTracking()
            .Include(tt => tt.IdBillingNavigation)
            .OrderByDescending(tt => tt.IdBillingNavigation.BillingDate)
            .Take(10)
            .Select(tt => new RecentActivityDto
            {
                Description = $"Venta de pasaje cod: {tt.TicketCode}",
                TimeAgo = "", // Handled in frontend or helper
                Type = "Sale",
                Date = tt.IdBillingNavigation.BillingDate.Value
            }).ToListAsync();

        foreach (var act in recentSales)
        {
            var diff = DateTime.Now - act.Date;
            if (diff.TotalMinutes < 60) act.TimeAgo = $"Hace {(int)diff.TotalMinutes} min";
            else if (diff.TotalHours < 24) act.TimeAgo = $"Hace {(int)diff.TotalHours} horas";
            else act.TimeAgo = $"Hace {(int)diff.TotalDays} días";
        }

        return new DashboardDto
        {
            DailyMetrics = new DailyMetricsDto
            {
                TodayIncome = todayIncome,
                YesterdayIncome = yesterdayIncome,
                IncomeTrendPercentage = incomeTrend,
                TodayTicketsSold = todayTickets,
                YesterdayTicketsSold = yesterdayTickets,
                TicketsTrendPercentage = ticketsTrend,
                OccupancyRate = avgOccupancy,
                TotalVehiclesInRoute = inRouteCount,
                TotalVehiclesInQueue = inQueueCount
            },
            TopRoutes = topRoutes,
            HourlyDemand = hourlyData,
            UpcomingDepartures = upcomingList,
            ActiveAlerts = allAlerts,
            RecentActivity = recentSales
        };
    }
}
