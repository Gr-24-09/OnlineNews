using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models;
using OnlineNews.Service;
using OnlineNews.Services;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, IRequestService requestService)
    {
        _logger = logger;
        _userService = userService;
        _requestService = requestService;
    }
    public async Task<IActionResult> Weather()
    {
        var weatherForecast = await _requestService.GetForecast("Sweden");
        return View(weatherForecast); 
    }

    public async Task<IActionResult> Index()
    {
        var result = await _userService.AddEmployee();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
