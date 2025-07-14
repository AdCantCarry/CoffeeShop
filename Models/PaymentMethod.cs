namespace CoffeeShop.Models;

public class PaymentMethod
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; // VNPAY, MOMO, COD, PayPal...
    public string Provider { get; set; } = "";

    public ICollection<Payment>? Payments { get; set; }
}
