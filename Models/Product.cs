using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Giá")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        // Thêm CategoryId & liên kết
        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
