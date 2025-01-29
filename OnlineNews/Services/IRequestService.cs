using OnlineNews.Models.API;

namespace OnlineNews.Services
{
    public interface IRequestService
    {
        Task<WeatherForecast> GetForecast(string city);
    }
}
