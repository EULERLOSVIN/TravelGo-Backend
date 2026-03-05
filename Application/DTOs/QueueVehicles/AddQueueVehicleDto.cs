namespace Application.DTOs.QueueVehicles
{
    public class AddQueueVehicleDto
    {
        public string DriverDni { get; set; } = string.Empty;
        public int IdTravelRoute { get; set; }
    }
}
