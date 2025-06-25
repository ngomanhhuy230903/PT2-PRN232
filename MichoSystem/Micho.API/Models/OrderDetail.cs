using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<OrderDetailIceCream> OrderDetailIceCreams { get; set; } = new List<OrderDetailIceCream>();
}
