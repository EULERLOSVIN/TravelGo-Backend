using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TypeDocument
{
    public int IdTypeDocument { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
