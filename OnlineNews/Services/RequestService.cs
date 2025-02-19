using Newtonsoft.Json;
using OnlineNews.Models.API;
using System.Net.Http;
using System.Text.Json;

namespace OnlineNews.Services
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient _httpClient;

        public RequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetForecast(string city)
        {
            var url = $"http://weatherapi.dreammaker-it.se/forecast?city={city}&lang=en";
            var forecast = await _httpClient.GetFromJsonAsync<WeatherForecast>(url);
            return forecast;
        }
        public async Task<List<WeatherForecast>> GetForecasts(List<string> cities)
        {
            var forecasts = new List<WeatherForecast>();

            foreach (var item in cities)
            {
                var url = $"http://weatherapi.dreammaker-it.se/forecast?city={item}&lang=en";
                var forecast = await _httpClient.GetFromJsonAsync<WeatherForecast>(url);
                if (forecast != null)
                {
                    forecasts.Add(forecast);
                }
            }
            return forecasts;
        }
        public async Task<ElectricitySpotPrice> GetData()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://spotprices.lexlink.se/espot");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ElectricitySpotPrice>(data)!;
                }
                else
                {
                    // Log an error or handle failure case as needed
                    return new ElectricitySpotPrice();
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                // For example, log using a logger: _logger.LogError(ex, "Error fetching electricity data");

                return new ElectricitySpotPrice(); // Return an empty object on failure
            }
        }


    }
}
