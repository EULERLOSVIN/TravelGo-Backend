namespace Application.DTOs.Vehicles;

public class DetailVehicleDto
{
    public int IdVehicle { get; set; }
    public string? PhotoBase64 { get; set; }
    public string Plate { get; set; }
    public string Type { get; set; }
    public int SeatNumber { get; set; }
    public string? Model { get; set; }

    public string Route { get; set; }
    public int IdRoute { get; set; }   // 👈 nuevo

    public string Driver { get; set; }
    public int IdDriver { get; set; }  // 👈 nuevo

    public string State { get; set; }
    public int IdState { get; set; }   // 👈 nuevo

    public DateOnly ExpirationSoatDate { get; set; }
}