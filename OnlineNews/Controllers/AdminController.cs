using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Security.Claims;


namespace OnlineNews.Controllers
{
    
    public class AdminController : Controller
    {

        private readonly UserManager<User> userManager;

        public AdminController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> ListUsers()
        { 
            var users = await userManager.Users.ToListAsync();
            return View(users); 
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
