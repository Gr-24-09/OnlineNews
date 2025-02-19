using OnlineNews.Models.API;

namespace OnlineNews.Services
{
    public interface IRequestService
    {
        Task<List<WeatherForecast>> GetForecasts(List<string> cities);
        Task<WeatherForecast> GetForecast(string city);
        Task<SpotPriceNow> GetData();
    }
}
