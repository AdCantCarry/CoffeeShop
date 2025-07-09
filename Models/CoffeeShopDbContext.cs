using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models;

public class CoffeeShopDbContext : DbContext
{
  public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options) : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Product> Products { get; set; }
public DbSet<Category> Categories { get; set; }

}
