namespace App.DTO.Payment
{
    public class PaymentResultDto
    {
        public bool IsSuccess { get; set; }
        public string PaymentId { get; set; } // ID của thanh toán nếu thành công
    }

}