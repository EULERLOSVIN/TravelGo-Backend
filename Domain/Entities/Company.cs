using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Company
{
    public int IdCompany { get; set; }

    public string? BusinessName { get; set; }

    public string? Ruc { get; set; }

    public string? FiscalAddress { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual ICollection<Headquarter> Headquarters { get; set; } = new List<Headquarter>();
}
