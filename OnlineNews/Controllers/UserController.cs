using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using OnlineNews.Models.ViewModels;
using OnlineNews.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace OnlineNews.Controllers
{
    [Authorize] 
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;

        public UserController(IUserService userService, ISubscriptionService subscriptionService)
        {
            _userService = userService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByIdAsync(userId);
            var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

            var model = new UserPageViewModel
            {
                User = user,
                Subscription = subscription
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSubscription(string subscriptionType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscriptionTypeEntity = await _subscriptionService.GetSubscriptionTypeByNameAsync(subscriptionType);

            if (subscriptionTypeEntity == null)
            {
                TempData["Error"] = "Invalid subscription type.";
                return RedirectToAction("MyPage");
            }

            var success = await _subscriptionService.ChangeSubscriptionTypeAsync(userId, subscriptionTypeEntity);

            if (success)
            {
                TempData["Message"] = "Subscription updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update subscription.";
            }

            return RedirectToAction("MyPage");
        }
    }
}