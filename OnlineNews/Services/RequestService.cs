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
    }
}
