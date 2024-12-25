namespace App.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }
    [ForeignKey("Product")]
    public int ProductID { get; set; }
    [ForeignKey("User")]
    [Required]
    public string UserId { get; set; }
    public User User { get; set; }

    public Review Rep_review { get; set; }
    public string Content { get; set; }

    public int Like { get; set; }
    public List<string> user_liked { get; set; } = new List<string>();

}
