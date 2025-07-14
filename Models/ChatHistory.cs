namespace CoffeeShop.Models;

public class ChatHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = "";
    public bool IsFromUser { get; set; }
    public DateTime SentAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
}
