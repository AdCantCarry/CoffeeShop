using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers;

public class AccountController : Controller
{
    private readonly CoffeeShopDbContext _context;

    public AccountController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // Nếu đã đăng nhập thì chuyển hướng theo vai trò
        var role = HttpContext.Session.GetString("Role");
        if (role == "Admin")
            return RedirectToAction("Index", "AdminDashboard"); // ✅ Sửa ở đây
        if (role == "User")
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password, string? returnUrl)
    {
        var user = _context.Users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);

        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return user.Role == "Admin"
                ? RedirectToAction("Index", "AdminDashboard")
                : RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "⚠ Sai tài khoản hoặc mật khẩu.";
        return View();
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
