using System;
using System.Collections.Generic;

namespace Persistence;

public partial class StateHeadquarter
{
    public int IdStateHeadquarter { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Headquarter> Headquarters { get; set; } = new List<Headquarter>();
}
