namespace CoffeeShop.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = ""; // "Admin", "User"
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Order>? Orders { get; set; }
    public ICollection<Address>? Addresses { get; set; }
}
