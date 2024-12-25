using App.DTO.Cart;
using App.Models;

namespace App.Mappers
{
    public static class CartMappers
    {
        public static CartDto toCartDto(this List<CartItem> Lmodel)
        {
            var Items = new List<CartItem>();

            foreach (var item in Lmodel)
            {
                var i = new CartItem
                {
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                };
                Items.Add(i);
            }

            return new CartDto
            {
                Items = Items
            };
        }

        public static CartItem toCartItem(this CartItemDto item, string id)
        {
            return new CartItem
            {
                UserId = id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }
    }
}