using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Micho.Web.ViewModels;
using System.Text.Json;

namespace Micho.Web.Pages.Invoices
{
    public class ViewModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ViewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public InvoiceViewModel Invoice { get; set; }
        public bool OrderFound { get; set; } = true;

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (orderId <= 0)
            {
                return RedirectToPage("/Invoices/Index");
            }

            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7281/api/orders/{orderId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Invoice = JsonSerializer.Deserialize<InvoiceViewModel>(jsonString, options);
                }
                else
                {
                    OrderFound = false;
                }
            }
            catch
            {
                OrderFound = false;
            }

            return Page();
        }
    }
}