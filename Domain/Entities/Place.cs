using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Place
{
    public int IdPlace { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TravelRoute> TravelRouteIdPlaceANavigations { get; set; } = new List<TravelRoute>();

    public virtual ICollection<TravelRoute> TravelRouteIdPlaceBNavigations { get; set; } = new List<TravelRoute>();
}
