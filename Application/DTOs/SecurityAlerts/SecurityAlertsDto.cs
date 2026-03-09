using System;
using System.Collections.Generic;

namespace Application.DTOs.SecurityAlerts;

public class SecurityAlertsDto
{
    public List<ExpiringSoatDto> ExpiringSoat { get; set; } = new();
    public List<ExpiringLicenseDto> ExpiringLicenses { get; set; } = new();
    public int TotalSoatAlerts { get; set; }
    public int TotalLicenseAlerts { get; set; }
}

public class ExpiringSoatDto
{
    public int IdVehicle { get; set; }
    public string UnitNumber { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public int DaysToExpiration { get; set; }
}

public class ExpiringLicenseDto
{
    public int IdPerson { get; set; }
    public string DriverName { get; set; } = null!;
    public string LicenseCategory { get; set; } = null!;
    public DateOnly ExpirationDate { get; set; }
    public int DaysToExpiration { get; set; }
}
