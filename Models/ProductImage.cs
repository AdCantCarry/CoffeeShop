namespace CoffeeShop.Models;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = "";

    public Product? Product { get; set; }
}
