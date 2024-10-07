using App.DTO.Cart;
using App.DTO.Order;
using App.Models;
namespace App.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> Order(string id);
        OrderDto GetOrderById(int id);
    }
}