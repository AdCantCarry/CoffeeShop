using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using Newtonsoft.Json;

namespace CoffeeShop.Controllers
{
    public class CartController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public CartController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            // ✅ Kiểm tra đã đăng nhập hay chưa
            if (HttpContext.Session.GetString("Username") == null)
            {
                // Chuyển hướng sang trang đăng nhập kèm returnUrl
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Cart/AddToCart/{id}" });
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var existing = cart.FirstOrDefault(c => c.ProductId == id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl ?? "",
                    Price = product.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
