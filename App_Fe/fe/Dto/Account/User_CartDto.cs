
using Models;

public class User_CartDto
{
    public string? Id { set; get; }
    public string? Username { set; get; }
    public string? Email { set; get; }
    public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
    public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
}
