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

            // Nếu đã có dữ liệu, không seed lại
            if (context.Users.Any() || context.Products.Any()) return;

            // Users
            context.Users.AddRange(
                new User { Username = "admin", Password = "admin123", Role = "Admin", FullName = "Admin User" },
                new User { Username = "user1", Password = "user123", Role = "User", FullName = "Regular User" }
            );

            // Categories
            var category = new Category { Name = "Coffee" };
            context.Categories.Add(category);

            // Products - Thêm 3 sản phẩm mẫu
            var product1 = new Product
            {
                Name = "Cà phê sữa đá",
                Description = "Cà phê Việt Nam truyền thống",
                Price = 25000,
                Category = category
            };

            var product2 = new Product
            {
                Name = "Bạc xỉu",
                Description = "Thức uống nhẹ nhàng với sữa nhiều hơn cà phê",
                Price = 30000,
                Category = category
            };

            var product3 = new Product
            {
                Name = "Espresso",
                Description = "Cà phê Ý đậm đặc",
                Price = 35000,
                Category = category
            };

            context.Products.AddRange(product1, product2, product3);

            // Toppings
            var topping = new Topping { Name = "Thạch cà phê", Price = 5000 };
            context.Toppings.Add(topping);

            // ProductTopping
            context.ProductToppings.Add(new ProductTopping
            {
                Product = product1,
                Topping = topping
            });

            // Payment Methods
            context.PaymentMethods.AddRange(
                new PaymentMethod { Name = "COD", Provider = "Cash" },
                new PaymentMethod { Name = "VNPAY", Provider = "VNPAY" },
                new PaymentMethod { Name = "MOMO", Provider = "MOMO" },
                new PaymentMethod { Name = "PAYPAL", Provider = "PAYPAL" }
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
