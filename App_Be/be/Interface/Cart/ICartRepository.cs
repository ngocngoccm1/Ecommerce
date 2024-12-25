using App.DTO.Cart;
using App.Models;
namespace App.Interface
{
    public interface ICartRepository
    {
        Task<CartDto> GetCart(string id);
        Task Add(CartItemDto cartItem, string id);
        Task Remove(int productId);
    }
}
