using App.DTO.Order;
using App.Models;
namespace App.Interface
{
    public interface IOrderRepository
    {
        Task<OrderDto> Create(string id, List<int> ids);
        Task<Order> GetById(int id);
        Task<List<Order>> GetAll(string userId);

    }
}