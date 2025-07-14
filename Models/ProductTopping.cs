namespace CoffeeShop.Models;

public class ProductTopping
{
    public int ProductId { get; set; }
    public int ToppingId { get; set; }

    public Product? Product { get; set; }
    public Topping? Topping { get; set; }
}
