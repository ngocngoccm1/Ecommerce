
using App.DTO.Product;

namespace App.DTO.Category
{

    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductDTO> Products { set; get; } = null;
    }
}



