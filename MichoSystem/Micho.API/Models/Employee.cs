using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;

    public string Idcard { get; set; } = null!;

    public string? Gender { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
