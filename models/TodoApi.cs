using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class TodoItem
{
    [Key]
    public long Id { get; set; }
    [StringLength(50)]
    [Required]
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}