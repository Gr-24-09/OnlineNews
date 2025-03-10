using Microsoft.AspNetCore.Mvc;
using OnlineNews.Services;
using OnlineNews.Data;
using OnlineNews.Models.ViewModels;
using OnlineNews.Models;
using OnlineNews.Models.Helper;
namespace OnlineNews.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ApplicationDbContext db, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _db = db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
            if (cartItems == null)
            {
                _logger.LogInformation("Loading Cart/Index view.");
                return View();
            }
            CartViewModel cart = _cartService.GetCartMovies(cartItems);

            return View(cart);
        }
        public IActionResult AddProductToCart(int id)
        {
            var product = _db.Products.FirstOrDefault(m => m.Id == id);
            if (product != null)
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
                _cartService.AddToCart(cartItems, product);
                HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);
                TempData["SuccessMessage"] = "Item added to cart successfuly";
            }

            // (In case of using outside of Cart Index View)
            // Get the referrer URL from the request headers 
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);  // Redirect back to the page the user was on
            }

            // If referer is not available, fallback to a default page (Home Index)
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProductFromCart(int id)
        {
            var movie = _db.Products.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
                _cartService.RemoveFromCart(cartItems, id);
                HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult LowerQuantity(int id)
        {
            var movie = _db.Products.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
                _cartService.LowerQuantity(cartItems, id);
                HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EmptyCart()
        {
            HttpContext.Session.Remove("CartItems");
            return View();
        }

        [HttpPost]
        public IActionResult ProceedToCheckout(CartViewModel cartViewModel)
        {
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var customer = cartViewModel.Customer;
            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.Now
            };
            foreach (var cartItem in cartItems)
            {
                var orderRow = new OrderRow
                {
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                order.OrderRows.Add(orderRow);
            }
            _db.Orders.Add(order);
            _db.SaveChanges();
            HttpContext.Session.Remove("CartItems");
            return RedirectToAction("Checkout", new { orderId = order.Id });
        }
        [HttpGet]
        public JsonResult GetCustomerByEmail(string email)
        {
            var customer = _db.Customers
      .Where(c => c.EmailAddress == email)  
      .FirstOrDefault();

            if (customer != null)
            {
                var customerData = new
                {
                    firstName = customer.FirstNameBillingAddress, 
                    lastName = customer.LastNameBillingAddress,  
                    billingAddress = customer.BillingAddress,
                    billingZip = customer.BillingZip,
                    billingCity = customer.BillingCity
                };
                return Json(customerData);
            }

            return Json(null);  
        }
        public IActionResult Checkout()
        {

            return View(); 
        }
    }
}
