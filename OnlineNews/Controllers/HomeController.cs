using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models;
using OnlineNews.Models.API;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISubscriptionService _subscriptionService;
    public HomeController(ILogger<HomeController> logger, IUserService userService, IRequestService requestService, ISubscriptionService subscriptionService, IArticleService articleService, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userService = userService;
        _requestService = requestService;
        _articleService = articleService;
        _httpContextAccessor = httpContextAccessor;
        _subscriptionService = subscriptionService;

    }
    public async Task<IActionResult> Index()
    {
        ViewBag.HasConsented = _articleService.HasConsented(_httpContextAccessor);
        var result = await _userService.AddEmployee();
        FrontPageViewModel obj = new FrontPageViewModel();
        obj.SomeLatestNews = _articleService.SomeLatestNews();
        obj.Mostpopular = _articleService.Mostpopular();
        obj.OneLatestNews = _articleService.OneLatestNews();
        obj.EditorsChoiceArticles = _articleService.EditorsChoice();
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
    [Authorize]
    public async Task<IActionResult> PremiumArticle()
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

        if (subscription != null && subscription.SubscriptionType?.TypeName == "Premium")
        {
            return View();
        }
        else
        {
            TempData["Error"] = "This article is only available for Premium subscribers.";
            return RedirectToAction("Index", "Home");
        }
    }

    [Authorize]
    public async Task<IActionResult> BasicArticle()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

        if (subscription != null && subscription.SubscriptionType?.TypeName == "Basic")
        {
            return View(); // Show Basic content
        }
        else
        {
            TempData["Error"] = "This article is only available for Basic subscribers.";
            return RedirectToAction("Index", "Home");
        }
    }

    [Authorize]
    public async Task<IActionResult> NonSubscriberArticle()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

        if (subscription == null) // User is not subscribed
        {
            return View(); // Show non-subscriber content or a prompt to subscribe
        }
        else
        {
            TempData["Error"] = "You are already subscribed.";
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> WeatherSearch(string city)
    {
        WeatherForecast weather = null;
        if (!string.IsNullOrEmpty(city))
        {
            weather = await _requestService.GetWeatherByCityNameAsync(city);
        }

        ViewData["City"] = city;
        return View(weather);

    }
    public async Task<IActionResult> BusinessData()
    {
        var data = await _requestService.GetPrices();
        return View(data);
    }

    [Authorize]
    public async Task<IActionResult> EditorsChoiced()
    {
        // Get the authenticated user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve the subscription for the authenticated user
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

        // Check if the user doesn't have a subscription
        if (subscription == null)
        {
            TempData["Error"] = "You don't have a subscription. Please subscribe to access premium content.";
            return RedirectToAction("Subscribe", "Subscription"); // Redirect to the Subscription page
        }

        // Check if the user is not a Premium subscriber
        if (subscription.SubscriptionType?.TypeName != "Premium")
        {
            TempData["Error"] = "You need a Premium subscription to view Editors' Choice articles.";
            return RedirectToAction("Index", "Home"); // Redirect to Home or another page
        }

        // Fetch the Editors' Choice articles (ensure that this method returns a list or collection of articles)
        var articles = _articleService.EditorsChoice();

        // Return the view with the articles
        return View(articles);
    }

    


}

