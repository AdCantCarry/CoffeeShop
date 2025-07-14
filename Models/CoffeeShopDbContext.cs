using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models
{
    public class CoffeeShopDbContext : DbContext
    {
        public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options)
            : base(options)
        {
        }

        // Người dùng & địa chỉ
        public DbSet<User> Users => Set<User>();
        public DbSet<Address> Addresses => Set<Address>();

        // Sản phẩm & danh mục
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Topping> Toppings => Set<Topping>();
        public DbSet<ProductTopping> ProductToppings => Set<ProductTopping>();

        // Đơn hàng & chi tiết
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        // Thanh toán & giao dịch
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        // Tin tức
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();

        // Chatbot
        public DbSet<ChatHistory> ChatHistories => Set<ChatHistory>();

        // Giảm giá & tồn kho
        public DbSet<Discount> Discounts => Set<Discount>();
        public DbSet<ProductDiscount> ProductDiscounts => Set<ProductDiscount>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<InventoryHistory> InventoryHistories => Set<InventoryHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Khóa chính kết hợp cho bảng nhiều-nhiều
            modelBuilder.Entity<ProductTopping>()
                .HasKey(pt => new { pt.ProductId, pt.ToppingId });

            modelBuilder.Entity<ProductDiscount>()
                .HasKey(pd => new { pd.ProductId, pd.DiscountId });

            // Quan hệ Product - ProductTopping
            modelBuilder.Entity<ProductTopping>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductToppings)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductTopping>()
                .HasOne(pt => pt.Topping)
                .WithMany()
                .HasForeignKey(pt => pt.ToppingId);

            // Quan hệ Product - Discount
            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Product)
                .WithMany()
                .HasForeignKey(pd => pd.ProductId);

            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Discount)
                .WithMany()
                .HasForeignKey(pd => pd.DiscountId);

            // Xử lý mối quan hệ Order - Payment
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            // Khóa ngoại InventoryHistory không cần navigation nếu không dùng
        }
    }
}
