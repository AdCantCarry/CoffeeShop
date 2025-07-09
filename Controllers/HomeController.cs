using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

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
            var products = _context.Products.ToList(); // lấy sản phẩm từ DB

            var viewModel = new HomeViewModel
            {
                Products = products
            };

            return View(viewModel);
        }
    }
}
