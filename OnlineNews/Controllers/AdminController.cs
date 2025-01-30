using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Security.Claims;


namespace OnlineNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<User> _userManager;

        
        public AdminController(IAdminService adminService, UserManager<User> userManager)
        { 
            _adminService = adminService;
            _userManager = userManager;
        }

        public async Task <IActionResult> Index()
        {
            
            var roleResult = await _adminService.CreateRole();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var assignRoleResult = await _adminService.AddRoleToEmployee(userId);
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
    }
}
