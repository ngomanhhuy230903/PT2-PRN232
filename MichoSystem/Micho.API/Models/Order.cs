using System;
using System.Collections.Generic;

namespace Micho.API.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public int CustomerId { get; set; }

    public int? EmpId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Emp { get; set; }

    public virtual ICollection<OrderDetailIceCream> OrderDetailIceCreams { get; set; } = new List<OrderDetailIceCream>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
