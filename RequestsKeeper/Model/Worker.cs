using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class Worker
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int? SubDivisionId { get; set; }

    public int? DepartmentId { get; set; }

    public int? Code { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual SubDivision? SubDivision { get; set; }

    public virtual ICollection<Visit> Visits { get; } = new List<Visit>();
}
