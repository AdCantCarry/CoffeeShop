using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
