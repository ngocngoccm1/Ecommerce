using App.DTO.Payment;
using App.Models;
namespace App.Interface
{
    public interface IPaymentService
    {
        PaymentResultDto ProcessPayment(PaymentDTO paymentDTO);
    }
}