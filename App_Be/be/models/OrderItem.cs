namespace App.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    [ForeignKey("Product")]
    public int ProductId { set; get; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]

    public decimal Price { get; set; }
}