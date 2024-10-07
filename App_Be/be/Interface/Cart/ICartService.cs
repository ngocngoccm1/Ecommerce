using App.DTO.Cart;
namespace App.Interface
{
    public interface ICartService
    {
        Task<CartDto> GetCart(string id);
        Task AddToCart(CartItemDto cartItem, string id);
        Task RemoveFromCart(int productId);
    }
}