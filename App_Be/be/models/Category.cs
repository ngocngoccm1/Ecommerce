namespace App.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }
}

