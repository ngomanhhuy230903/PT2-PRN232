namespace Micho.API.ViewModels
{
    public class AnalyticsViewModel
    {
        public List<BestSellingProductViewModel> BestSellingProducts { get; set; }
        public List<PeakHourViewModel> PeakOrderHours { get; set; }
    }

    public class BestSellingProductViewModel
    {
        public int IceCreamId { get; set; }
        public string IceCreamName { get; set; }
        public int TotalQuantitySold { get; set; }
    }

    public class PeakHourViewModel
    {
        public int Hour { get; set; }
        public int OrderCount { get; set; }
    }
}