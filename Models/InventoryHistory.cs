namespace CoffeeShop.Models;

public class InventoryHistory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ChangedByUserId { get; set; }
    public int QuantityChanged { get; set; }
    public string Reason { get; set; } = "";
    public DateTime ChangedAt { get; set; } = DateTime.Now;
}
