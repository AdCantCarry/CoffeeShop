using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers;

public class AdminDashboardController : Controller
{
    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("Role");

        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        return View("~/Views/Admin/AdminDashboard/Index.cshtml");
    }
}
