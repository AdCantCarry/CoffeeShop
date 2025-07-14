using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models.ViewModels;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public HomeController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Take(6) // Lấy vài sản phẩm nổi bật
                .ToList();

            var viewModel = new HomeViewModel
            {
                Products = products
            };

            return View(viewModel);
        }

        public IActionResult Products()
        {
            var products = _context.Products
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            return View("/Views/Home/Products.cshtml", products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Where(p => p.Id == id)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    ProductToppings = p.ProductToppings
                })
                .FirstOrDefault();

            if (product == null) return NotFound();

            var toppingIds = product.ProductToppings.Select(pt => pt.ToppingId).ToList();
            var toppings = _context.Toppings
                .Where(t => toppingIds.Contains(t.Id))
                .ToList();

            ViewBag.Toppings = toppings;
            return View("/Views/Home/Details.cshtml", product);
        }
    }
}
