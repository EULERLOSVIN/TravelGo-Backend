using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Seat
{
    public int IdSeat { get; set; }

    public int Number { get; set; }

    public virtual ICollection<SeatVehicle> SeatVehicles { get; set; } = new List<SeatVehicle>();
}
