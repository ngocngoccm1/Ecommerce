
using App.DTO.Cart;
using App.DTO.Order;
using App.Interface;
using App.Mappers;
using App.Models;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> Create(string id, List<int> ids)
    {


        var collection = _context.CartItems
                        .Where(o => o.UserId == id)
                        .Include(o => o.Product)
                        .ToList();

        var collectioN = new List<CartItem>();
        foreach (var i in ids)
        {
            foreach (var item in collection)
            {
                if (i == item.ProductId)
                {
                    collectioN.Add(item);
                    _context.CartItems.Remove(item);

                }
            }
        }
        var newOrder = collectioN.CreateOrder(id);
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        return newOrder.toOrderDto();
    }

    public async Task<List<Order>> GetAll(string userId)
    {
        var orders = await _context.Orders.Where(o => o.UserId == userId)
                                    .Include(o => o.OrderItems)

                                    .ToListAsync();
        if (orders != null)
        {
            return orders;
        }
        return new List<Order>();
    }

    public async Task<Order> GetById(int id)
    {
        var order = await _context.Orders
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                .Include(o => o.Payment) // Include related OrderItems
                                .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return new Order();

        return order;
    }

}
