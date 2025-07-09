using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers.Admin
{
    public class CategoryController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public CategoryController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        // GET: /Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View("~/Views/Admin/Category/Index.cshtml", categories);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/Category/Create.cshtml");
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["Success"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/Category/Create.cshtml", category);
        }

        // GET: /Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            return View("~/Views/Admin/Category/Edit.cshtml", category);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["Success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/Category/Edit.cshtml", category);
        }

        // POST: /Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["Success"] = "Xóa danh mục thành công!";
            return RedirectToAction("Index");
        }
    }
}
