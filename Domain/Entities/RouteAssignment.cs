using System;
using System.Collections.Generic;

namespace Persistence;

public partial class RouteAssignment
{
    public int IdRouteAssignment { get; set; }

    public int IdPerson { get; set; }

    public int IdTravelRoute { get; set; }

    public DateOnly? AssignmentDate { get; set; }

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual TravelRoute IdTravelRouteNavigation { get; set; } = null!;
}
