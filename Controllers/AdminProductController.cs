using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public AdminProductController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: /AdminProduct
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View("/Views/Admin/AdminProduct/Index.cshtml", products);
        }

        // GET: /AdminProduct/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View("/Views/Admin/AdminProduct/Create.cshtml");
        }

        // POST: /AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View("/Views/Admin/AdminProduct/Create.cshtml", product);
        }
    }
}
