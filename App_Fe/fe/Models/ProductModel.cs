using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ProductModel
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string? Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Giá")]
        [DataType(DataType.Currency)] // Định dạng kiểu tiền tệ
        public decimal Price { get; set; }

        [Display(Name = "Số lượng tồn kho")]
        public int Stock { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Mã danh mục")]
        public required int categoryID { get; set; }

        [Display(Name = "Tên danh mục")]
        public string? CategoryName { get; set; }

        [Display(Name = "Danh sách đánh giá")]
        public List<ReviewModel> reviews { get; set; } = new List<ReviewModel>();

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)] // Hiển thị kiểu ngày giờ
        public DateTime CreatedAt { get; set; }
    }
}
