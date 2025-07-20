using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddDbContext<CoffeeShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Set default culture to Vietnamese
var cultureInfo = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // 🔥 Đảm bảo middleware Session luôn được đặt trước khi dùng
app.UseMiddleware<CoffeeShop.Middlewares.AuthRedirectMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Gọi seed dữ liệu
// Gọi seed dữ liệu nếu chưa có dữ liệu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CoffeeShopDbContext>();

    // Chỉ seed nếu chưa có sản phẩm nào
    if (!context.Products.Any())
    {
        SeedData.Initialize(services);
    }
}

app.Run();
