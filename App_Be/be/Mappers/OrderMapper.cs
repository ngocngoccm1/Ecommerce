using App.DTO.Order;
using App.Models;

namespace App.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto toOrderDto(this Order orderM)
        {
            var Items = new List<OrderItemDto>();
            foreach (var item in orderM.OrderItems)
            {
                Items.Add(toOrderItemDto(item));
            }
            return new OrderDto
            {
                OrderDate = orderM.OrderDate,
                TotalAmount = orderM.TotalPrice,
                OrderItems = Items
            };
        }
        public static List<OrderDto> toOdersDTO(this List<Order> orders)
        {
            var Items = new List<OrderDto>();
            foreach (var item in orders)
            {
                Items.Add(item.toOrderDto());
            }
            return Items;
        }
        public static OrderItemDto toOrderItemDto(this OrderItem orderIM)
        {
            return new OrderItemDto
            {
                ProductId = orderIM.ProductId,
                Quantity = orderIM.Quantity
            };
        }
        public static Order CreateOrder(this List<CartItem> model, string id)
        {
            var Items = new List<OrderItem>();
            decimal TotalPrice = 0;

            foreach (var item in model)
            {
                var i = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                Items.Add(i);
                TotalPrice = TotalPrice + item.Product.Price;
            }
            return new Order
            {
                // Map các thuộc tính từ OrderDto sang Order
                OrderDate = DateTime.Now,
                OrderItems = Items,
                UserId = id,
                TotalPrice = TotalPrice,
                Payment = null
            };
        }

    }
}