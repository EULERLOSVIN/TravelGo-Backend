namespace Application.DTOs.QueueVehicles
{
    public class DriverQueueInfoDto
    {
        public string DriverFullName { get; set; } = string.Empty;
        public string DriverDni { get; set; } = string.Empty;
        public int IdVehicle { get; set; }
        public string VehiclePlate { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;

        // Rutas asignadas
        public List<DriverRouteDto> AssignedRoutes { get; set; } = new List<DriverRouteDto>();
    }

    public class DriverRouteDto
    {
        public int IdRoute { get; set; }
        public string DestinationName { get; set; } = string.Empty;
    }
}
