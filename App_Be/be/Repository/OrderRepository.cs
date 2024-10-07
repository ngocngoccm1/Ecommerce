
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

    public async Task<OrderDto> Create(string id)
    {


        var collection = _context.CartItems
                        .Where(o => o.UserId == id)
                        .Include(o => o.Product)
                        .ToList();


        var newOrder = collection.CreateOrder(id);


        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        return newOrder.toOrderDto();
    }


    public OrderDto GetById(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null) return null;

        return order.toOrderDto();
    }

}
