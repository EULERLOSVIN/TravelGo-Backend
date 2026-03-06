using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleState
{
    public int IdVehicleState { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
