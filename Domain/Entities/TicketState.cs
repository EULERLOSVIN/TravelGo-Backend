using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TicketState
{
    public int IdTicketState { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TravelTicket> TravelTickets { get; set; } = new List<TravelTicket>();
}
