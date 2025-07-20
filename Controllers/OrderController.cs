using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using CoffeeShop.Helpers;
using Newtonsoft.Json;

namespace CoffeeShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public OrderController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.PaymentMethods = _context.PaymentMethods.ToList();
            ViewBag.Total = cart.Sum(i => i.Price * i.Quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Checkout(string paymentMethod = "COD")
        {
            var userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = JsonConvert.DeserializeObject<User>(userJson);
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            decimal total = cart.Sum(item => item.Quantity * item.Price);

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Pending"
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            var method = _context.PaymentMethods.FirstOrDefault(p => p.Name == paymentMethod);
            if (method == null)
            {
                TempData["Error"] = "Phương thức thanh toán không hợp lệ!";
                return RedirectToAction("Checkout");
            }

            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentMethodId = method.Id,
                Amount = total,
                Status = "Pending"
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");
            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
