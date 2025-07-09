using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CoffeeShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseSession(); // phải có
app.UseMiddleware<CoffeeShop.Middlewares.AuthRedirectMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CoffeeShopDbContext>();
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Username = "admin", Password = "123", Role = "Admin" },
            new User { Username = "user", Password = "123", Role = "User" }
        );
        db.SaveChanges();
    }
}
app.UseStaticFiles(); // Cho phép dùng file tĩnh như CSS

app.Run();
