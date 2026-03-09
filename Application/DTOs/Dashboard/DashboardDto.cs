using System;
using System.Collections.Generic;

namespace Application.DTOs.Dashboard;

public class DashboardDto
{
    public DailyMetricsDto DailyMetrics { get; set; } = new();
    public List<RouteProfitabilityDto> TopRoutes { get; set; } = new();
    public List<HourlyDemandDto> HourlyDemand { get; set; } = new();
    public List<UpcomingDepartureDto> UpcomingDepartures { get; set; } = new();
    public List<ActiveAlertDto> ActiveAlerts { get; set; } = new();
    public List<RecentActivityDto> RecentActivity { get; set; } = new();
}

public class DailyMetricsDto
{
    public decimal TodayIncome { get; set; }
    public decimal YesterdayIncome { get; set; }
    public int IncomeTrendPercentage { get; set; }

    public int TodayTicketsSold { get; set; }
    public int YesterdayTicketsSold { get; set; }
    public int TicketsTrendPercentage { get; set; }

    public int OccupancyRate { get; set; }
    
    public int TotalVehiclesInRoute { get; set; }
    public int TotalVehiclesInQueue { get; set; }
}

public class RouteProfitabilityDto
{
    public string RouteName { get; set; } = null!;
    public decimal TotalIncome { get; set; }
    public int PercentageOfTotal { get; set; }
}

public class HourlyDemandDto
{
    public string Hour { get; set; } = null!;
    public int TicketsSold { get; set; }
}

public class UpcomingDepartureDto
{
    public string Hour { get; set; } = null!;
    public string RouteName { get; set; } = null!;
    public string DriverName { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public int OccupancyPercentage { get; set; }
    public string Status { get; set; } = null!; // Listo, Cargando, Baja ocupacion, etc.
}

public class ActiveAlertDto
{
    public string Type { get; set; } = null!; // SOAT, Licencia
    public string Description { get; set; } = null!;
    public string Severity { get; set; } = null!; // High, Medium, Low
}

public class RecentActivityDto
{
    public string Description { get; set; } = null!;
    public string TimeAgo { get; set; } = null!;
    public string Type { get; set; } = null!; // Sale, Route, User
    public DateTime Date { get; set; }
}
