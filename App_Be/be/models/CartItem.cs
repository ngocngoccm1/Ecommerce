using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual User User { get; set; }
}
