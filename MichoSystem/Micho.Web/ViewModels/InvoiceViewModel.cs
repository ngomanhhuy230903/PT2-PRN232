namespace Micho.Web.ViewModels
{
    public class InvoiceViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public List<InvoiceItemViewModel> Items { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class InvoiceItemViewModel
    {
        public string IceCreamName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}