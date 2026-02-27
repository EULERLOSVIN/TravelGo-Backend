namespace Application.DTOs.Vehicles;

public class CreateVehicleDto
{
    public string Plate { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Seats { get; set; }

    public int? DriverId { get; set; }
    public DateOnly? SoatExpiry { get; set; }
}