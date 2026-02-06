using System;
using System.Collections.Generic;

namespace Persistence;

public partial class QueueVehicle
{
    public int IdQueueVehicle { get; set; }

    public int IdPlace { get; set; }

    public int IdVehicle { get; set; }

    public DateTime EntryDate { get; set; }

    public virtual Place IdPlaceNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
