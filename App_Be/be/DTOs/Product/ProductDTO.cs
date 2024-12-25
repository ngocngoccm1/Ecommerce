using App.DTO.Category;
using App.Models;
namespace App.DTO.Product
{

    public class ProductDTO
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}



