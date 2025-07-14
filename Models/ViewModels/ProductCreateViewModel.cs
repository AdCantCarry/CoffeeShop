using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        // Thông tin cơ bản của sản phẩm
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }

        // Danh sách hình ảnh được upload
        public List<IFormFile>? ImageFiles { get; set; }
        public string? ImageUrl { get; set; }
        // Các topping được chọn (danh sách ID)
        public List<int>? SelectedToppingIds { get; set; }
        public string? ExistingImageUrl { get; set; }
        public List<Topping>? AllToppings { get; set; }
        public List<Category>? AllCategories { get; set; }
    }
}
