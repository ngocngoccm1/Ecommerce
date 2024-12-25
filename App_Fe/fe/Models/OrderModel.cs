namespace Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();

        public Payment Payment { get; set; } = new Payment();
        public decimal TotalPrice { get; set; }

    }

    public class Payment
    {
        public string? Id { get; set; }


        public decimal Amount { get; set; }
        public string Status { get; set; } = "Chưa thanh toán";

        public DateTime PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }

    }

}
