using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Person
{
    public int IdPerson { get; set; }

    public int IdTypeDocument { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string NumberIdentityDocument { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual TypeDocument IdTypeDocumentNavigation { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<RouteAssignment> RouteAssignments { get; set; } = new List<RouteAssignment>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
