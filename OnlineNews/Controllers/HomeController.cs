using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models;
using OnlineNews.Service;
using System.Diagnostics;

namespace OnlineNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            var result = _userService =  userService;
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
}
