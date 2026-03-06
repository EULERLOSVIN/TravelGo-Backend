using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Headquarter
{
    public int IdHeadquarter { get; set; }

    public int IdCompany { get; set; }

    public int IdStateHeadquarter { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public bool IsMain { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual StateHeadquarter IdStateHeadquarterNavigation { get; set; } = null!;
}
