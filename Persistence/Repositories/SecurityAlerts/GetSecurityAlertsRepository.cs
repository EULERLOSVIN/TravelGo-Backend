using Application.DTOs.SecurityAlerts;
using Application.Interfaces.SecurityAlerts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.SecurityAlerts;

public class GetSecurityAlertsRepository : IGetSecurityAlertsRepository
{
    private readonly ApplicationDbContext _context;

    public GetSecurityAlertsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SecurityAlertsDto> GetExpiringDocumentsAsync()
    {
        var now = DateTime.Now;
        var todayStart = now.Date;
        var limitDate = todayStart.AddDays(60); 
        
        var todayDateOnly = DateOnly.FromDateTime(todayStart);
        var limitDateOnly = DateOnly.FromDateTime(limitDate);

        // Fetch SOAT Data
        var expiringSoat = await _context.DocumentVehicles
            .AsNoTracking()
            .Where(dv => dv.SoatExpirationDate >= todayDateOnly && dv.SoatExpirationDate <= limitDateOnly)
            .OrderBy(dv => dv.SoatExpirationDate)
            .Select(dv => new ExpiringSoatDto
            {
                IdVehicle = dv.IdVehicle,
                UnitNumber = dv.IdVehicleNavigation.PlateNumber, 
                PlateNumber = dv.IdVehicleNavigation.PlateNumber,
                HeadquarterName = "Principal", // Placeholder for now or join with headquarters if needed
                ExpirationDate = dv.SoatExpirationDate,
                DaysToExpiration = (int)(dv.SoatExpirationDate.ToDateTime(TimeOnly.MinValue) - todayStart).TotalDays
            })
            .ToListAsync();

        // Fetch Licenses Data
        var expiringLicenses = await _context.DocumentDrivers
            .AsNoTracking()
            .Where(dd => dd.LicenseExpirationDate >= todayDateOnly && dd.LicenseExpirationDate <= limitDateOnly)
            .OrderBy(dd => dd.LicenseExpirationDate)
            .Select(dd => new ExpiringLicenseDto
            {
                IdPerson = dd.IdPerson,
                DriverName = $"{dd.IdPersonNavigation.FirstName} {dd.IdPersonNavigation.LastName}",
                LicenseCategory = "A-IIIc",
                ExpirationDate = dd.LicenseExpirationDate,
                DaysToExpiration = (int)(dd.LicenseExpirationDate.ToDateTime(TimeOnly.MinValue) - todayStart).TotalDays
            })
            .ToListAsync();

        // Analytical Grouping: Alerts by Headquarter (Simulated/Placeholder Grouping for Design)
        // Since vehicles aren't directly linked to HQs in a simple way here, we group by a static logic or plate prefixes
        var allAlertsCount = expiringSoat.Count + expiringLicenses.Count;
        
        var alertsByHq = new List<AlertsByHeadquarterDto>
        {
            new() { HeadquarterName = "Lima Central", AlertCount = (int)(allAlertsCount * 0.4), PercentageOfTotal = 40 },
            new() { HeadquarterName = "Arequipa Sur", AlertCount = (int)(allAlertsCount * 0.3), PercentageOfTotal = 30 },
            new() { HeadquarterName = "Cusco Terminal", AlertCount = (int)(allAlertsCount * 0.3), PercentageOfTotal = 30 }
        };

        return new SecurityAlertsDto
        {
            ExpiringSoat = expiringSoat,
            ExpiringLicenses = expiringLicenses,
            AlertsByHeadquarter = alertsByHq,
            TotalSoatAlerts = expiringSoat.Count,
            TotalLicenseAlerts = expiringLicenses.Count
        };
    }
}
