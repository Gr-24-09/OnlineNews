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
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userService.GetUserByIdAsync(userId);
                var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("Index", "Home"); // Redirecting to Home or error page
                }

                var model = new UserPageViewModel
                {
                    User = user,
                    Subscription = subscription
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching user data.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSubscription(string subscriptionType)
        {
            if (string.IsNullOrEmpty(subscriptionType))
            {
                TempData["Error"] = "Subscription type is required.";
                return RedirectToAction("MyPage");
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch the subscription type by name
                var subscriptionTypeEntity = await _subscriptionService.GetSubscriptionTypeByNameAsync(subscriptionType);

                if (subscriptionTypeEntity == null)
                {
                    TempData["Error"] = "Invalid subscription type.";
                    return RedirectToAction("MyPage");
                }

                // Attempt to change the subscription
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
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while changing the subscription.";
                return RedirectToAction("MyPage");
            }
        }
    }
}
