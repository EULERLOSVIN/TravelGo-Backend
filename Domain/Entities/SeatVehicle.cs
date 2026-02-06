using System;
using System.Collections.Generic;

namespace Persistence;

public partial class SeatVehicle
{
    public int IdSeatVehicle { get; set; }

    public int IdVehicle { get; set; }

    public int IdSeat { get; set; }

    public int IdStateSeatVehicle { get; set; }

    public virtual Seat IdSeatNavigation { get; set; } = null!;

    public virtual StateSeatVehicle IdStateSeatVehicleNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
