using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
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
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Checkout(decimal totalAmount)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson ?? "[]");

            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "Cart");
            }

            // Mặc định gán UserId = 1 nếu chưa có Auth
            int userId = int.TryParse(User.Identity?.Name, out var id) ? id : 1;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderItems = cart.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentMethodId = _context.PaymentMethods.FirstOrDefault(p => p.Name == "COD")?.Id ?? 1,
                Status = "Pending",
                Amount = totalAmount,
                PaidAt = DateTime.Now
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
