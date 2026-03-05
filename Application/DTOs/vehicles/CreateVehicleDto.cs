namespace Application.DTOs.Vehicles;

public class CreateVehicleDto
{
    // ===============================
    // VEHICLE
    // ===============================
    public string Plate { get; set; } = null!;
    public string? Model { get; set; }

    // ACTIVO / INACTIVO (1 o 2 según tu tabla VehicleState)
    public int IdVehicleState { get; set; }

    // Conductor (si es "Sin asignar" manda null o 0)
    public int? IdPerson { get; set; }

    // ===============================
    // DETAIL VEHICLE
    // ===============================
    public string VehicleType { get; set; } = null!;  // Mini Van, Auto, etc.
    public int SeatNumber { get; set; }

    // ===============================
    // DOCUMENT VEHICLE (SOAT)
    // ===============================
    public DateOnly SoatExpiry { get; set; }  // Se guardará como Date

    // ===============================
    // IMAGEN
    // ===============================
    public byte[]? MainPhoto { get; set; }
}