using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StateAccount
{
    public int IdStateAccount { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
