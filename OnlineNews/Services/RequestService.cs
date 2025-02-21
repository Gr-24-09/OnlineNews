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
        public async Task<WeatherForecast> GetWeatherByCityNameAsync(string cityName)
        {

            var url = $"http://weatherapi.dreammaker-it.se/forecast?city={cityName}&lang=en";
            var forecast = await _httpClient.GetFromJsonAsync<WeatherForecast>(url);
            return forecast;
        }

        public async Task<SpotPriceNow> GetData()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://spotprices.lexlink.se/espot");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SpotPriceNow>(data)!;
                }
                else
                {
                    return new SpotPriceNow();
                }
            }
            catch (Exception ex)
            {
                return new SpotPriceNow(); 
            }
        }


    }
}
