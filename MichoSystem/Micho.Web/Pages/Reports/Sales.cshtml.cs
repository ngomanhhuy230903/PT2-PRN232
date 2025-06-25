using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Micho.Web.ViewModels;
using System.Text.Json;

namespace Micho.Web.Pages.Reports
{
    public class SalesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SalesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public SalesReportViewModel Report { get; set; }

        public async Task OnGetAsync()
        {
            // Gán giá trị mặc định nếu là lần đầu truy cập
            if (StartDate == null)
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (EndDate == null)
            {
                EndDate = DateTime.Now;
            }

            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                var startDateString = StartDate.Value.ToString("yyyy-MM-dd");
                var endDateString = EndDate.Value.ToString("yyyy-MM-dd");
                var requestUrl = $"https://localhost:7281/api/reports/sales?startDate={startDateString}&endDate={endDateString}";

                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Report = JsonSerializer.Deserialize<SalesReportViewModel>(jsonString, options);
                }
            }
            catch (Exception)
            {
                // Xử lý lỗi
                Report = null;
            }
        }
    }
}