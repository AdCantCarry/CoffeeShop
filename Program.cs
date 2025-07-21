using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews().AddNewtonsoftJson();

// Kết nối cơ sở dữ liệu
builder.Services.AddDbContext<CoffeeShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Cấu hình session đúng cách
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);           // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true;                            // Ngăn chặn JavaScript đọc cookie
    options.Cookie.IsEssential = true;                         // Cookie vẫn hoạt động trong chế độ privacy cao
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ✅ Đặt định dạng mặc định là tiếng Việt
var cultureInfo = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // 🔥 Luôn đặt trước Auth, Middleware, v.v.

app.UseMiddleware<CoffeeShop.Middlewares.AuthRedirectMiddleware>();

app.UseAuthorization();

// Định tuyến
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Gọi seed dữ liệu nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CoffeeShopDbContext>();

    if (!context.Products.Any())
    {
        SeedData.Initialize(services);
    }
}

app.Run();
