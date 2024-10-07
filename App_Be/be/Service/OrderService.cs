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

    public Task<OrderDto> Order(string id)
    {
        return _orderRepository.Create(id);
    }

    public OrderDto GetOrderById(int id)
    {
        return _orderRepository.GetById(id);
    }

}
