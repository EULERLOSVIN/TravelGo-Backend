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

        // New Detailed fields
        public int OccupiedSeats { get; set; }
        public int TotalSeats { get; set; }
        public string ScheduledDepartureTime { get; set; } = string.Empty;
        public int RemainingMinutes { get; set; }
        public string Status { get; set; } = string.Empty; // "EN TURNO" / "EN COLA"
    }
}
