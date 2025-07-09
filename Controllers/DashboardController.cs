using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
            return RedirectToAction("Index", "Home");

        return View();
    }
}
