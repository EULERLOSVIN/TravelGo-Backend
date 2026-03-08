using System;
using System.Collections.Generic;

namespace Persistence;

public partial class DocumentDriver
{
    public int IdDocumentDriver { get; set; }

    public int IdPerson { get; set; }

    public DateOnly LicenseExpirationDate { get; set; }

    public virtual Person IdPersonNavigation { get; set; } = null!;
}
