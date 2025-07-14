namespace CoffeeShop.Models;

public class Transaction
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public string Status { get; set; } = "";
    public string? GatewayResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Payment? Payment { get; set; }
}
