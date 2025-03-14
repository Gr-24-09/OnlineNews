using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Data;
using Microsoft.EntityFrameworkCore;


namespace OnlineNews.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public SubscriptionController(ISubscriptionService subscriptionService, UserManager<User> userManager, ApplicationDbContext db)
        {
            _subscriptionService = subscriptionService;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string subscriptionType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Subscription newSubscription = new Subscription()
            {
                Subscriber = user,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(30),
                PaymentComplete = false, 
                //SubscriptionType = new SubscriptionType { TypeName = subscriptionType }
            };


            await _subscriptionService.AddSubscriptionAsync(newSubscription, subscriptionType);

            TempData["Message"] = "Subscription created successfully! Please complete the payment.";
            return RedirectToAction("DummyPayment");
        }

        [HttpGet]
        public IActionResult DummyPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DummyPayment(string cardNumber, string expiryDate, string cvv)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var subscription = await _subscriptionService.PaymentConfirmation(userId);

            if (subscription != null)
            {
                subscription.PaymentComplete = true;
                TempData["Message"] = "Payment Successful! You are now an active subscriber.";
            }
            else
            {
                TempData["Error"] = "Failed to complete payment.";
            }


            return RedirectToAction("MyPage", "User");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.DateofBirth = updatedUser.DateofBirth;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Profile updated successfully!";
                    return RedirectToAction("MyPage", "User");
                }

                TempData["Error"] = "Failed to update profile.";
            }

            return View(updatedUser);
        }




    }
}