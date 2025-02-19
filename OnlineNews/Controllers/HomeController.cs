using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models;
using OnlineNews.Service;
using OnlineNews.Services;
using System.Diagnostics;
using System.Security.Claims;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly IArticleService _articleService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, IRequestService requestService,IArticleService articleService)
    {
        _logger = logger;
        _userService = userService;
        _requestService = requestService;
        _articleService = articleService;
    }
    public async Task<IActionResult> Weather()
    {
        var cities = new List<string> { "Stockholm", "Malmö", "Linköping", "Norrköping", "Uppsala", "Gothenburg", "Helsingborg", "Örebro", "Jönköping" ,"Lund", "Västerås", "Gotland" };
        var forecasts = await _requestService.GetForecasts(cities);
        return View(forecasts);
    }
    public async Task<IActionResult> Electricity()
    {
        var data = await _requestService.GetData();
        return View(data);
    }
    public async Task<IActionResult> Index()
    {
        var result = await _userService.AddEmployee();
        FrontPageViewModel obj = new FrontPageViewModel();
        obj.SomeLatestNews = _articleService.SomeLatestNews();
        obj.Mostpopular = _articleService.Mostpopular();
        obj.OneLatestNews = _articleService.OneLatestNews();
        return View(obj);
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

    public IActionResult Claims()
    {
        var user = HttpContext.User;
        var claims = user.Claims;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (user.IsInRole("Admin"))
        {
            return NotFound("Deactivated");
        }
        return RedirectToAction("ListUsers");
    }
    public IActionResult EditorsChoiced()
    {
        var articles1 = _articleService.EditorsChoice();
        return View(articles1);
    }
}
