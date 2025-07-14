namespace CoffeeShop.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PaymentMethodId { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed
    public decimal Amount { get; set; }
    public string? TransactionCode { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.Now;

    public Order? Order { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}
