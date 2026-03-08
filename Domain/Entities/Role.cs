using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Role
{
    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
