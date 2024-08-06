namespace App.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public Payment Payment { get; set; }
}
