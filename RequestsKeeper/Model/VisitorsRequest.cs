using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class VisitorsRequest
{
    public int Id { get; set; }

    public int? VisitorsId { get; set; }

    public int? RequestId { get; set; }

    public virtual Request? Request { get; set; }

    public virtual Visitor? Visitors { get; set; }
}
