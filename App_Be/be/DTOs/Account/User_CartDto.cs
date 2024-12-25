using System.ComponentModel.DataAnnotations;
using App.Models;




public class User_CartDto
{
    public string Id { set; get; }
    public string Username { set; get; }
    public string Email { set; get; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
