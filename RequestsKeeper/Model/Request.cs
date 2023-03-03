using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class Request
{
    public int Id { get; set; }

    public int TypeRequestId { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime FinishDate { get; set; } = DateTime.Now;

    public string TargetVisit { get; set; } = null!;

    public int WorkerId { get; set; }

    public int StatusId { get; set; }

    public string? Cause { get; set; }

    public int UsersId { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual TypeRequest TypeRequest { get; set; } = null!;

    public virtual User Users { get; set; } = null!;

    public virtual ICollection<VisitorsRequest> VisitorsRequests { get; } = new List<VisitorsRequest>();

    public virtual Worker Worker { get; set; } = null!;
}
