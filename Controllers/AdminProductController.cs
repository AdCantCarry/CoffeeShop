using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers.Admin
{
    [Area("Admin")]
    public class AdminProductController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public AdminProductController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/AdminProduct/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Create");
            }
            return View(product);
        }
    }
}
