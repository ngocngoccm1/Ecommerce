namespace App.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string Description { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Image { get; set; }
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }

    [Required]
    public Category Category { get; set; }

}


