using Microsoft.AspNetCore.Authorization;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(ILogger<HomeController> logger, IUserService userService, IRequestService requestService,IArticleService articleService,IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userService = userService;
        _requestService = requestService;
        _articleService = articleService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Weather()
    {

        var weatherForecast = await _requestService.GetForecast("Linköping");
        return View(weatherForecast); 
    }


    public async Task<IActionResult> Index()
    {
        // Check if the user has consented
        ViewBag.HasConsented = _articleService.HasConsented(_httpContextAccessor);
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

    [Authorize]
    public class NewsController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public NewsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

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

    public IActionResult EditorsChoiced()
    {
        var articles1 = _articleService.EditorsChoice();
        return View(articles1);
    }
    [HttpPost]
    public IActionResult AcceptCookies()
    {
        // Accept cookies and set the cookie consent status
        _articleService.AcceptCookies(_httpContextAccessor);
        TempData["Message"] = "You have accepted cookies.";
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult DeclineCookies()
    {
        _articleService.DeclineCookies(_httpContextAccessor);
        TempData["Message"] = "You have declined cookies.";
        return RedirectToAction("Index");

    }
}
