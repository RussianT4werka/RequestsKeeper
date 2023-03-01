using System;
using System.Collections.Generic;

namespace RequestsKeeper.Model;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Login { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual ICollection<Visitor> Visitors { get; } = new List<Visitor>();
}
