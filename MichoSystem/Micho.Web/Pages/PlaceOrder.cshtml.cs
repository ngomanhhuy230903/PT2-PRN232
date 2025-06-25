using Microsoft.AspNetCore.Mvc.RazorPages;
using Micho.Web.ViewModels;
using System.Text.Json;

namespace Micho.Web.Pages
{
    public class PlaceOrderModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlaceOrderModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<IceCreamViewModel> IceCreamList { get; set; } = new List<IceCreamViewModel>();

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            try
            {
                var response = await httpClient.GetAsync("https://localhost:7281/api/icecreams");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    IceCreamList = JsonSerializer.Deserialize<List<IceCreamViewModel>>(jsonString, options);
                }
            }
            catch (Exception)
            {
                IceCreamList = new List<IceCreamViewModel>();
            }
        }
    }
}