namespace CoffeeShop.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    public ICollection<ProductImage>? Images { get; set; }
    public ICollection<ProductTopping>? ProductToppings { get; set; }
}
