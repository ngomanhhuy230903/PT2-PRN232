using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class IceCream
{
    public int IceCreamId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Flavor { get; set; }

    public string? Color { get; set; }

    public virtual ICollection<OrderDetailIceCream> OrderDetailIceCreams { get; set; } = new List<OrderDetailIceCream>();
}
