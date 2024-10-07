
using App.DTO.Cart;
using App.Interface;
using App.Mappers;
using App.Models;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> GetCart(string id)
    {
        // Lấy thông tin giỏ hàng từ cơ sở dữ liệu
        var collection = await _context.CartItems
                            .Where(o => o.UserId == id)
                            .ToListAsync();

        var cartDto = collection.toCartDto();

        return cartDto;
    }

    public async Task Add(CartItemDto cartItem, string id)
    {
        var item = cartItem.toCartItem(id);
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(int productId)
    {
        var item = await _context.CartItems
        .FirstOrDefaultAsync(ci => ci.ProductId == productId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }


}
