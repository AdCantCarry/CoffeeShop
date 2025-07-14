namespace CoffeeShop.Models;

public class ArticleCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public ICollection<Article>? Articles { get; set; }
}

