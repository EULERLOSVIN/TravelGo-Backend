namespace Application.DTOs.Vehicles;

public class VehicleListItemDto
{
    public string UnitId { get; set; } = "";
    public string Plate { get; set; } = "";
    public string Model { get; set; } = "";
    public string Driver { get; set; } = "Sin asignar";
    public bool IsActive { get; set; }      // true=ACTIVO false=INACTIVO
    public bool SoatOk { get; set; }        // por ahora simple
}
