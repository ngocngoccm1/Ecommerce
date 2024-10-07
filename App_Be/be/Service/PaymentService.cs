using App.DTO.Payment;
using App.Interface;
using App.Models;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public PaymentResultDto ProcessPayment(PaymentDTO paymentDTO)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid().ToString(),
            OrderId = paymentDTO.OrderID,
            Amount = _paymentRepository.GetTotalPrice(paymentDTO.OrderID),
            PaymentMethod = paymentDTO.PaymentMethod,
            Status = "PENDING",
            PaymentDate = DateTime.UtcNow
        };

        _paymentRepository.CreatePaymentAsync(payment).Wait();
        return new PaymentResultDto
        {
            IsSuccess = true,
            PaymentId = payment.PaymentId
        };
    }


}
