using App.DTO.Cart;
using App.DTO.Order;
using App.Models;
namespace App.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> Order(string id, List<int> ids);
        Task<List<Order>> GetAll(string userId);
        Task<Order> GetOrderById(int id);
    }
}