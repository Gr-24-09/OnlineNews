using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Security.Claims;


namespace OnlineNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IAdminService _adminService;

        public AdminController(UserManager<User> userManager, IAdminService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
        }
        public async Task<IActionResult> ListUsers()
        { 
            var users = await _userManager.Users.ToListAsync();
            return View(users); 
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
        public async Task<IActionResult> RemoveRoleFromUser(string userId) 
        {
            await _adminService.RemoveAdminRoleFromEmployee(userId);
            return RedirectToAction(nameof(ListUsers));
        }

        public async Task<IActionResult> AddRoleToUser(string userId)
        {
            await _adminService.AddAdminRoleToEmployee(userId);
            return RedirectToAction(nameof(ListUsers));
        }
        //[Authorize(Roles = "Admin")]
        //private readonly IAdminService _adminService;
        //private readonly UserManager<User> _userManager;


        //public AdminController(IAdminService adminService, UserManager<User> userManager)
        //{ 
        //    _adminService = adminService;
        //    _userManager = userManager;
        //}

        //public async Task <IActionResult> Index()
        //{

        //    var roleResult = await _adminService.CreateRole();
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var assignRoleResult = await _adminService.AddRoleToEmployee(userId);
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}


    }
}
