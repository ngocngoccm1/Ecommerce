using App.Models;

namespace App.DTO.Cart
{
    public class CartDto
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

}