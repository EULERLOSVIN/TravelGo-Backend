using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class SeatVehicle
{
    public int IdSeatVehicle { get; set; }

    public int IdVehicle { get; set; }

    public int IdSeat { get; set; }

    public bool? StateSeat { get; set; }

    public virtual Seat IdSeatNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
