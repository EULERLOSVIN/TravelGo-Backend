using System.Collections.Generic;

namespace Application.Features.QueueManagement.Queries.GetDriverQueueInfo;

public class GetDriverQueueInfoResponse
{
    public int IdPerson { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int IdVehicle { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public List<RouteDto> AssignedRoutes { get; set; } = new();
}

public class RouteDto
{
    public int IdTravelRoute { get; set; }
    public string NameRoute { get; set; } = string.Empty;
}
