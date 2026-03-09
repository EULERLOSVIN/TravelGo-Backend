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
        var today = DateOnly.FromDateTime(DateTime.Now);
        var nextMonth = today.AddMonths(1);

        var expiringSoat = await _context.DocumentVehicles
            .AsNoTracking()
            .Include(dv => dv.IdVehicleNavigation)
            .Where(dv => dv.SoatExpirationDate <= nextMonth && dv.SoatExpirationDate >= today)
            .OrderBy(dv => dv.SoatExpirationDate)
            .Select(dv => new ExpiringSoatDto
            {
                IdVehicle = dv.IdVehicle,
                UnitNumber = dv.IdVehicleNavigation.PlateNumber, 
                PlateNumber = dv.IdVehicleNavigation.PlateNumber,
                ExpirationDate = dv.SoatExpirationDate,
                DaysToExpiration = (int)(dv.SoatExpirationDate.ToDateTime(TimeOnly.MinValue) - today.ToDateTime(TimeOnly.MinValue)).TotalDays
            })
            .ToListAsync();

        var expiringLicenses = await _context.DocumentDrivers
            .AsNoTracking()
            .Include(dd => dd.IdPersonNavigation)
            .Where(dd => dd.LicenseExpirationDate <= nextMonth && dd.LicenseExpirationDate >= today)
            .OrderBy(dd => dd.LicenseExpirationDate)
            .Select(dd => new ExpiringLicenseDto
            {
                IdPerson = dd.IdPerson,
                DriverName = $"{dd.IdPersonNavigation.FirstName} {dd.IdPersonNavigation.LastName}",
                LicenseCategory = "A-I", 
                ExpirationDate = dd.LicenseExpirationDate,
                DaysToExpiration = (int)(dd.LicenseExpirationDate.ToDateTime(TimeOnly.MinValue) - today.ToDateTime(TimeOnly.MinValue)).TotalDays
            })
            .ToListAsync();

        return new SecurityAlertsDto
        {
            ExpiringSoat = expiringSoat,
            ExpiringLicenses = expiringLicenses,
            TotalSoatAlerts = expiringSoat.Count,
            TotalLicenseAlerts = expiringLicenses.Count
        };
    }
}
