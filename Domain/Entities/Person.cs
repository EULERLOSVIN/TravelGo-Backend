using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Person
{
    public int IdPerson { get; set; }

    public int IdTypeDocument { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string NumberIdentityDocument { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual TypeDocument IdTypeDocumentNavigation { get; set; } = null!;
}
