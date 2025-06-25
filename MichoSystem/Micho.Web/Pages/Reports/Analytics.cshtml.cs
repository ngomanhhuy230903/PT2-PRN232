using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Micho.Web.ViewModels;
using System.Text.Json;

namespace Micho.Web.Pages.Reports
{
    public class AnalyticsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AnalyticsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public AnalyticsViewModel AnalyticsData { get; set; }

        public int MaxOrderCount { get; set; } = 0;

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                string requestUrl;
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    var startDateString = StartDate.Value.ToString("yyyy-MM-dd");
                    var endDateString = EndDate.Value.ToString("yyyy-MM-dd");
                    requestUrl = $"https://localhost:7281/api/reports/analytics?startDate={startDateString}&endDate={endDateString}";
                }
                else
                {
                    requestUrl = "https://localhost:7281/api/reports/analytics";
                }

                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    AnalyticsData = JsonSerializer.Deserialize<AnalyticsViewModel>(jsonString, options);

                    if (AnalyticsData?.PeakOrderHours?.Any() == true)
                    {
                        MaxOrderCount = AnalyticsData.PeakOrderHours.Max(h => h.OrderCount);
                    }
                }
            }
            catch (Exception)
            {
                AnalyticsData = null;
            }
        }
    }
}