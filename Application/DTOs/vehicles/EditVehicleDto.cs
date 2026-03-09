
namespace Application.DTOs.vehicles
{
    public class EditVehicleDto
    {
        public int IdVehicle { get; set; } 
        public string? MainPhoto { get; set; }
        public string Plate { get; set; } = null!;
        public string VehicleType { get; set; } = null!;
        public int SeatNumber { get; set; }
        public string? Model { get; set; }
        public int IdDriver { get; set; }
        public int IdState { get; set; }
        public DateOnly SoatExpirationDate { get; set; }
        public int IdRoute { get; set; }
    }
}
