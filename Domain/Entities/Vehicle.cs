using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Vehicle
{
    public int IdVehicle { get; set; }

    public int IdVehicleState { get; set; }

    public int IdPerson { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int? Model { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<DetailVehicle> DetailVehicles { get; set; } = new List<DetailVehicle>();

    public virtual ICollection<DocumentVehicle> DocumentVehicles { get; set; } = new List<DocumentVehicle>();

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual VehicleState IdVehicleStateNavigation { get; set; } = null!;

    public virtual ICollection<QueueVehicle> QueueVehicles { get; set; } = new List<QueueVehicle>();

    public virtual ICollection<TravelTicket> TravelTickets { get; set; } = new List<TravelTicket>();
}
