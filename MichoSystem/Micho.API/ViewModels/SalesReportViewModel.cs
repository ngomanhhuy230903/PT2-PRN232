namespace Micho.API.ViewModels
{
    public class SalesReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItemsSold { get; set; }
        public List<SalesReportItemViewModel> Orders { get; set; }
    }

    public class SalesReportItemViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public decimal OrderTotal { get; set; }
        public string Status { get; set; }
    }
}