using System;
using System.Collections.Generic;

namespace Persistence;

public partial class TravelTicket
{
    public int IdTravelTicket { get; set; }

    public int IdBilling { get; set; }

    public int IdTravelRoute { get; set; }

    public int IdTicketState { get; set; }

    public int IdVehicle { get; set; }

    public int SeatNumber { get; set; }

    public DateOnly? TravelDate { get; set; }

    public string TicketCode { get; set; } = null!;

    public virtual Billing IdBillingNavigation { get; set; } = null!;

    public virtual TicketState IdTicketStateNavigation { get; set; } = null!;

    public virtual TravelRoute IdTravelRouteNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
