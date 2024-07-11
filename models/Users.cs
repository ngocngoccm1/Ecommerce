namespace App.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public Cart Cart { get; set; }
}
