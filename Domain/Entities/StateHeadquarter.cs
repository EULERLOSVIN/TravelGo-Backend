using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StateHeadquarter
{
    public int IdStateHeadquarter { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Headquarter> Headquarters { get; set; } = new List<Headquarter>();
}
