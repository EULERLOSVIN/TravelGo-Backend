using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Account
{
    public int IdAccount { get; set; }

    public int IdRole { get; set; }

    public int IdPerson { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
