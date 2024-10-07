
using App.DTO.Cart;
using App.Interface;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CartDto> GetCart(string id)
    {
        return await _cartRepository.GetCart(id);
    }

    public async Task AddToCart(CartItemDto cartItem, string id)
    {
        await _cartRepository.Add(cartItem, id);
    }

    public async Task RemoveFromCart(int productId)
    {
        await _cartRepository.Remove(productId);
    }


}
