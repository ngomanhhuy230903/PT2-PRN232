using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class OrderDetailIceCream
{
    public int OrderDetailIceCreamId { get; set; }

    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int IceCreamId { get; set; }

    public virtual IceCream IceCream { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
