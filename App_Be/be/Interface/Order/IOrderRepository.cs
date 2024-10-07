using App.DTO.Order;
using App.Models;
namespace App.Interface
{
    public interface IOrderRepository
    {
        Task<OrderDto> Create(string id);
        OrderDto GetById(int id);
    }
}