using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Billing
{
    public int IdBilling { get; set; }

    public int IdPerson { get; set; }

    public int IdCompany { get; set; }

    public int IdPaymentMethod { get; set; }

    public DateTime? BillingDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual PaymentMethod IdPaymentMethodNavigation { get; set; } = null!;

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual ICollection<TravelTicket> TravelTickets { get; set; } = new List<TravelTicket>();
}
