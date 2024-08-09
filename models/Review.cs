namespace App.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    // [ForeignKey("User")]
    // public string UserId { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product Product { get; set; }
    // public User User { get; set; }
}

