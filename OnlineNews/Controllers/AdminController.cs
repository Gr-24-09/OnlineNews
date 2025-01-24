using Microsoft.AspNetCore.Mvc;
using OnlineNews.Data;
using OnlineNews.Service;

namespace OnlineNews.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        { 
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
    }
}
