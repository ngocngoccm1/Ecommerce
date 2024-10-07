using System.Security.Claims;
using App.DTO.Cart;
using App.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            var cart = await _cartService.GetCart(id);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItem)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            await _cartService.AddToCart(cartItem, userId);
            return Ok("Đã thêm vào giỏ hàng");
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _cartService.RemoveFromCart(productId);
            return Ok();
        }

    }


}