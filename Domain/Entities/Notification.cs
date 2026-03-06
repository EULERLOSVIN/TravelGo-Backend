using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Notification
{
    public int IdNotification { get; set; }

    public int IdTrip { get; set; }

    public int IdPerson { get; set; }

    public int IdNotificationState { get; set; }

    public string? Message { get; set; }

    public virtual NotificationState IdNotificationStateNavigation { get; set; } = null!;

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual Trip IdTripNavigation { get; set; } = null!;
}
