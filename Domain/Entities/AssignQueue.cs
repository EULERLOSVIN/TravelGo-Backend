using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AssignQueue
{
    public int IdAssignQueue { get; set; }

    public int IdQueueVehicle { get; set; }

    public int IdVehicle { get; set; }

    public virtual QueueVehicle IdQueueVehicleNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;
}
