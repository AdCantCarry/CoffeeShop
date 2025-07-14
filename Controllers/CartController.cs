using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using CoffeeShop.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Controllers
{
    public class CartController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public CartController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Tính tổng tiền
            ViewBag.Total = cart.Sum(c => c.Price * c.Quantity);

            return View(cart);
        }

        // POST: /Cart/Add
        [HttpPost]
        public IActionResult Add(int productId, int quantity, string action)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Lưu thông tin tạm thời vào TempData để xử lý sau khi đăng nhập
                TempData["PendingAddProductId"] = productId;
                TempData["PendingAddQuantity"] = quantity;
                TempData["PendingAddAction"] = action;
                return RedirectToAction("Login", "Account");
            }

            return ProcessAdd(productId, quantity, action);
        }

        // Sau khi login xong, chuyển hướng về đây để thêm lại sản phẩm
        public IActionResult AddAfterLogin()
        {
            if (TempData["PendingAddProductId"] != null && TempData["PendingAddQuantity"] != null)
            {
                int productId = (int)TempData["PendingAddProductId"];
                int quantity = (int)TempData["PendingAddQuantity"];
                string action = TempData["PendingAddAction"]?.ToString() ?? "add";

                return ProcessAdd(productId, quantity, action);
            }

            return RedirectToAction("Index", "Home");
        }

        private IActionResult ProcessAdd(int productId, int quantity, string action)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            if (action == "buy")
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", "Home", new { id = productId });
        }

        // GET: /Cart/Remove/5
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            int count = cart.Sum(c => c.Quantity);
            return Json(count);
        }

        // GET: /Cart/Clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
