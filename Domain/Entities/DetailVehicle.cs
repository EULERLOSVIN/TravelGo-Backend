using System;
using System.Collections.Generic;

namespace Persistence;

public partial class DetailVehicle
{
    public int IdDetailVehicle { get; set; }

    public int IdVehicle { get; set; }

    public int SeatNumber { get; set; }

    public string? VehicleType { get; set; }

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
