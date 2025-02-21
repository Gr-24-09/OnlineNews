using Microsoft.AspNetCore.Mvc;
using OnlineNews.Services;  
using OnlineNews.Models.API; 

namespace OnlineNews.ViewComponents
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly IRequestService _requestService;

        public WeatherViewComponent(IRequestService requestService)
        {
            _requestService = requestService; 
        }

        public async Task<IViewComponentResult> InvokeAsync(string city="Linköping")
        {
            var weather = await _requestService.GetWeatherByCityNameAsync(city);
            return View(weather); 
        }

    }
}
