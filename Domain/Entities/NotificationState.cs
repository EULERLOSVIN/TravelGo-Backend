using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class NotificationState
{
    public int IdNotificationState { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
