using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CoffeeShop.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
{
    using var context = new CoffeeShopDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<CoffeeShopDbContext>>());

    // Clear tất cả data cũ nếu muốn
    context.Users.RemoveRange(context.Users);
    context.Categories.RemoveRange(context.Categories);
    context.Products.RemoveRange(context.Products);
    context.Toppings.RemoveRange(context.Toppings);
    context.ProductToppings.RemoveRange(context.ProductToppings);
    context.PaymentMethods.RemoveRange(context.PaymentMethods);
    context.Articles.RemoveRange(context.Articles);
    context.ArticleCategories.RemoveRange(context.ArticleCategories);
    context.SaveChanges(); // Xóa xong rồi mới seed

    // Users
    context.Users.AddRange(
        new User { Username = "admin", Password = "admin123", Role = "Admin", FullName = "Admin User" },
        new User { Username = "user1", Password = "user123", Role = "User", FullName = "Regular User" }
    );

    // Categories
    var category = new Category { Name = "Coffee" };
    context.Categories.Add(category);

    // Products
    var product = new Product
    {
        Name = "Cà phê sữa đá",
        Description = "Cà phê Việt Nam truyền thống",
        Price = 25000,
        Category = category
    };
    context.Products.Add(product);

    // Toppings
    var topping = new Topping { Name = "Thạch cà phê", Price = 5000 };
    context.Toppings.Add(topping);

    // ProductTopping
    context.ProductToppings.Add(new ProductTopping
    {
        Product = product,
        Topping = topping
    });

    // Payment Methods
    context.PaymentMethods.AddRange(
        new PaymentMethod { Name = "COD", Provider = "Cash" },
        new PaymentMethod { Name = "VNPAY", Provider = "VNPAY" },
        new PaymentMethod { Name = "MOMO", Provider = "MOMO" }
    );

    // Articles
    var newsCat = new ArticleCategory { Name = "Cà phê kiến thức" };
    context.ArticleCategories.Add(newsCat);
    context.Articles.Add(new Article
    {
        Title = "Nguồn gốc cà phê Việt Nam",
        Content = "Bài viết chi tiết về nguồn gốc cà phê...",
        Category = newsCat
    });

    context.SaveChanges();
}

    }
}
