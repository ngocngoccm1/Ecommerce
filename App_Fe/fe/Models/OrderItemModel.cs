namespace Models
{

    public class OrderItemModel
    {
        public int Id { get; set; }

        public int ProductId { set; get; }
        public ProductModel? Product { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}