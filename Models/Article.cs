namespace CoffeeShop.Models;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime PublishedAt { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }

    public ArticleCategory? Category { get; set; }
}
