namespace App.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    [Key]
    public string PaymentId { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    [Column(TypeName = "decimal(18,2)")]

    public decimal Amount { get; set; }
    public string Status { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; }

    public virtual Order Order { get; set; }
}
