using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class Visitor
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime DoB { get; set; }

    public int PassportSeries { get; set; }

    public int PassportNumber { get; set; }

    public byte[]? Photo { get; set; }

    public string? Organisation { get; set; }

    public string Note { get; set; } = null!;

    public byte[]? ScanPassport { get; set; }

    public int UsersId { get; set; }

    public virtual User Users { get; set; } = null!;

    public virtual ICollection<VisitorsRequest> VisitorsRequests { get; } = new List<VisitorsRequest>();

    public virtual ICollection<VisitorsVisit> VisitorsVisits { get; } = new List<VisitorsVisit>();
}
