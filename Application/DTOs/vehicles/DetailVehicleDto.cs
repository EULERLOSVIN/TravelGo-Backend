namespace Application.DTOs.Vehicles;

public class DetailVehicleDto
{
    public int IdVehicle { get; set; }
    public string? PhotoBase64 { get; set; }
    public required string Type { get; set; }
    public required string Plate { get; set; }
    public required string Route { get; set; }
    public required string Driver { get; set; }
    public required string  State { get; set; }
    public DateOnly ExpirationSoatDate { get; set; }
}
