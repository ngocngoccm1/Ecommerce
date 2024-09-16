namespace App.DTO.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}