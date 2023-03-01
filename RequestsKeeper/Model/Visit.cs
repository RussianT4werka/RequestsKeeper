using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class Visit
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int WorkerId { get; set; }

    public string? GroupNumber { get; set; }

    public virtual ICollection<VisitorsVisit> VisitorsVisits { get; } = new List<VisitorsVisit>();

    public virtual Worker Worker { get; set; } = null!;
}
