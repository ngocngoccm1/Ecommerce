namespace Models
{

    public class ProductModel
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public required int categoryID { get; set; }
        public string? CategoryName { set; get; }
        public DateTime CreatedAt { get; set; }

    }


}