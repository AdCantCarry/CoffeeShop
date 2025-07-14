using CoffeeShop.Models;
using CoffeeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminProductController(CoffeeShopDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View("/Views/Admin/AdminProduct/Index.cshtml", products);
        }

        public IActionResult Create()
        {
            var vm = new ProductCreateViewModel
            {
                AllCategories = _context.Categories.ToList(),
                AllToppings = _context.Toppings.ToList()
            };
            return View("/Views/Admin/AdminProduct/Create.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllCategories = _context.Categories.ToList();
                vm.AllToppings = _context.Toppings.ToList();
                return View("/Views/Admin/AdminProduct/Create.cshtml", vm);
            }

            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Quantity = vm.Quantity, // ✅ thêm số lượng
                CategoryId = vm.CategoryId
            };

            if (vm.ImageFiles != null && vm.ImageFiles.Any())
            {
                var file = vm.ImageFiles.First();
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            if (vm.SelectedToppingIds != null)
            {
                foreach (var tid in vm.SelectedToppingIds)
                {
                    _context.ProductToppings.Add(new ProductTopping
                    {
                        ProductId = product.Id,
                        ToppingId = tid
                    });
                }
                _context.SaveChanges();
            }

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductToppings)
                .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var vm = new ProductCreateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity, // ✅ lấy số lượng
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageUrl,
                SelectedToppingIds = product.ProductToppings.Select(pt => pt.ToppingId).ToList(),
                AllCategories = _context.Categories.ToList(),
                AllToppings = _context.Toppings.ToList()
            };

            return View("/Views/Admin/AdminProduct/Edit.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductCreateViewModel vm)
        {
            var product = _context.Products.Include(p => p.ProductToppings).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.AllCategories = _context.Categories.ToList();
                vm.AllToppings = _context.Toppings.ToList();
                return View("/Views/Admin/AdminProduct/Edit.cshtml", vm);
            }

            product.Name = vm.Name;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.Quantity = vm.Quantity; // ✅ cập nhật số lượng
            product.CategoryId = vm.CategoryId;

            if (vm.ImageFiles != null && vm.ImageFiles.Any())
            {
                var file = vm.ImageFiles.First();
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                product.ImageUrl = "/images/" + fileName;
            }

            var oldToppings = _context.ProductToppings.Where(t => t.ProductId == id);
            _context.ProductToppings.RemoveRange(oldToppings);
            if (vm.SelectedToppingIds != null)
            {
                foreach (var tid in vm.SelectedToppingIds)
                {
                    _context.ProductToppings.Add(new ProductTopping
                    {
                        ProductId = id,
                        ToppingId = tid
                    });
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductToppings)
                .ThenInclude(pt => pt.Topping)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();
            return View("/Views/Admin/AdminProduct/Details.cshtml", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var fullPath = Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _context.ProductToppings.RemoveRange(_context.ProductToppings.Where(t => t.ProductId == id));
            _context.Products.Remove(product);
            _context.SaveChanges();

            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}
