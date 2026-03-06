using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Assignment
{
    public int IdAssignment { get; set; }

    public int IdAccount { get; set; }

    public int IdHeadquarter { get; set; }

    public DateTime? AssignmentDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Account IdAccountNavigation { get; set; } = null!;

    public virtual Headquarter IdHeadquarterNavigation { get; set; } = null!;
}
