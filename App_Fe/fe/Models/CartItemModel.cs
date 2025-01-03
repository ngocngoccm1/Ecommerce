namespace Models
{


    public class CartItemModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required ProductModel Product { get; set; }
        public string? UserId { get; set; }
    }

}