using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StateTrip
{
    public int IdStateTrip { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
