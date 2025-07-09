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
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);

        if (user != null)
        {
            // Lưu phiên đăng nhập
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            // Điều hướng theo vai trò
            return user.Role == "Admin"
                ? RedirectToAction("Index", "AdminDashboard") // ✅ Sửa ở đây
                : RedirectToAction("Index", "Home");
        }

        // Báo lỗi nếu sai tài khoản hoặc mật khẩu
        ViewBag.Error = "⚠ Sai tài khoản hoặc mật khẩu.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
