using System;
using System.Collections.Generic;

namespace Persistence;

public partial class DepartureTime
{
    public int IdDepartureTime { get; set; }

    public int IdTravelRoute { get; set; }

    public TimeOnly Hour { get; set; }

    public virtual TravelRoute IdTravelRouteNavigation { get; set; } = null!;
}
