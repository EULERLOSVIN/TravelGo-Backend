using System;
using System.Collections.Generic;

namespace Persistence;

public partial class TravelRoute
{
    public int IdTravelRoute { get; set; }

    public int IdPlaceA { get; set; }

    public int IdPlaceB { get; set; }

    public string NameRoute { get; set; } = null!;

    public decimal? Price { get; set; }

    public virtual Place IdPlaceANavigation { get; set; } = null!;

    public virtual Place IdPlaceBNavigation { get; set; } = null!;

    public virtual ICollection<RouteAssignment> RouteAssignments { get; set; } = new List<RouteAssignment>();

    public virtual ICollection<TravelTicket> TravelTickets { get; set; } = new List<TravelTicket>();
}
