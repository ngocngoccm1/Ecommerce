using App.Models;
namespace App.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> GetPaymentByIdAsync(string id);
        Task<List<Payment>> GetPaymentsByStatusAsync(string status);
        decimal GetTotalPrice(int id);
    }
}