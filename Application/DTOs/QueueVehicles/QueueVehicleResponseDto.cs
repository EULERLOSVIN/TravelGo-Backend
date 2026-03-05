namespace Application.DTOs.QueueVehicles
{
    public class QueueVehicleResponseDto
    {
        public int IdAssignQueue { get; set; }
        public int Turn { get; set; }
        public string DriverFullName { get; set; } = string.Empty;
        public string DriverDni { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public int IdRoute { get; set; }
        public string DestinationName { get; set; } = string.Empty;
    }
}
