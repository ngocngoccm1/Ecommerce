namespace Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ICollection<ProductModel>? Products { get; set; }
    }
}



