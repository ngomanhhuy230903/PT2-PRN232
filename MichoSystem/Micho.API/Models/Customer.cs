using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
