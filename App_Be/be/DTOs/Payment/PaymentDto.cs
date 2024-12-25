namespace App.DTO.Payment
{
    public class PaymentDTO
    {
        public int OrderID { get; set; }
        // public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class OrderModel
    {
        public int Id { get; set; }  // ID của đơn hàng
        public DateTime OrderDate { get; set; }  // Ngày đặt hàng
        public string UserId { get; set; }  // ID người dùng
        public UserModel User { get; set; }  // Thông tin người dùng (nếu cần thiết)
        public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();  // Các sản phẩm trong đơn hàng
        public PaymentModel Payment { get; set; }  // Thông tin thanh toán
        public decimal TotalPrice { get; set; }  // Tổng giá trị của đơn hàng
    }

    public class OrderItemModel
    {
        public int Id { get; set; }  // ID của mục trong đơn hàng
        public int OrderId { get; set; }  // ID đơn hàng
        public int ProductId { get; set; }  // ID sản phẩm
        public ProductModel Product { get; set; }  // Thông tin sản phẩm (nếu cần thiết)
        public int Quantity { get; set; }  // Số lượng sản phẩm
        public decimal Price { get; set; }  // Giá sản phẩm
    }

    public class ProductModel
    {
        public int Id { get; set; }  // ID sản phẩm
        public string Name { get; set; }  // Tên sản phẩm
        public decimal Price { get; set; }  // Giá sản phẩm
    }

    public class PaymentModel
    {
        public int? PaymentId { get; set; }  // ID thanh toán (có thể null)
        public int OrderId { get; set; }  // ID đơn hàng
        public decimal Amount { get; set; }  // Số tiền thanh toán
        public string Status { get; set; }  // Trạng thái thanh toán (ví dụ: "Chưa thanh toán")
        public DateTime PaymentDate { get; set; }  // Ngày thanh toán
        public string PaymentMethod { get; set; }  // Phương thức thanh toán
        public OrderModel Order { get; set; }  // Thông tin đơn hàng
    }

    public class UserModel
    {
        public string Id { get; set; }  // ID người dùng
        public string Name { get; set; }  // Tên người dùng
    }
}


