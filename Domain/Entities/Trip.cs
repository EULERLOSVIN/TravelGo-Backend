using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Trip
{
    public int IdTrip { get; set; }

    public int IdStateTrip { get; set; }

    public DateTime? DepartureDate { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public int IdVehicle { get; set; }

    public virtual StateTrip IdStateTripNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
