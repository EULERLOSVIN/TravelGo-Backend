using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class DocumentVehicle
{
    public int IdDocumentVehicle { get; set; }

    public int IdVehicle { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
