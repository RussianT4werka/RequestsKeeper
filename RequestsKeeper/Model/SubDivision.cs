using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class SubDivision
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; } = new List<Worker>();
}
