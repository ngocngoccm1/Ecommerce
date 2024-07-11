namespace App.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    [Column(TypeName = "decimal(18,2)")]

    public decimal Amount { get; set; }

    public Order Order { get; set; }
}
