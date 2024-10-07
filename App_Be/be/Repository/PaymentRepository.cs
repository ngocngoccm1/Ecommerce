using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        await _context.Set<Payment>().AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetPaymentByIdAsync(string id)
    {
        return await _context.Set<Payment>().FindAsync(id);
    }
    public decimal GetTotalPrice(int id)
    {
        return _context.Orders.Where(o => o.Id == id).Select(o => o.TotalPrice).FirstOrDefault();
    }
    public async Task<List<Payment>> GetPaymentsByStatusAsync(string status)
    {
        return await _context.Set<Payment>().Where(p => p.Status == status).ToListAsync();
    }
}