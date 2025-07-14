namespace CoffeeShop.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public int Quantity { get; set; } // ✅ thêm dòng này

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<ProductTopping>? ProductToppings { get; set; }
    public ICollection<ProductDiscount>? ProductDiscounts { get; set; }
}

