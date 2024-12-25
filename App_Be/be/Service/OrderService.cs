using App.DTO.Cart;
using App.DTO.Order;
using App.Interface;
using App.Models;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<OrderDto> Order(string id, List<int> ids)
    {
        return _orderRepository.Create(id, ids);
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await _orderRepository.GetById(id);
    }

    public async Task<List<Order>> GetAll(string userId)
    {
        return await _orderRepository.GetAll(userId);
    }
}
