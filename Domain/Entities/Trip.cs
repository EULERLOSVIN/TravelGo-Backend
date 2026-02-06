using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Trip
{
    public int IdTrip { get; set; }

    public int IdStateTrip { get; set; }

    public int IdTravelTicket { get; set; }

    public DateTime? DepartureDate { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public virtual StateTrip IdStateTripNavigation { get; set; } = null!;

    public virtual TravelTicket IdTravelTicketNavigation { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
