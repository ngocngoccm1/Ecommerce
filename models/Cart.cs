namespace App.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    [Key]
    public int CartId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
    
    public ICollection<CartItem> CartItems { get; set; }
}

