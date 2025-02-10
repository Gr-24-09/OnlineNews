using Microsoft.AspNetCore.Mvc;

namespace OnlineNews.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
