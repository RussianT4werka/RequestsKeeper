using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class TypeRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
