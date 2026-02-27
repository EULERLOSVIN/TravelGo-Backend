using System;
using System.Collections.Generic;

namespace Persistence;

public partial class QueueVehicle
{
    public int IdQueueVehicle { get; set; }

    public int Number { get; set; }

    public virtual ICollection<AssignQueue> AssignQueues { get; set; } = new List<AssignQueue>();
}
