using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    // [Key]
    // public string UserId { get; set; }

    // [Required]
    // public string Username { get; set; }

    // [Required]
    // public string Email { get; set; }

    // [Required]
    // public string PasswordHash { get; set; }

    // [Required]
    // public string FirstName { get; set; }

    // [Required]
    // public string LastName { get; set; }

    // public string Address { get; set; }
    // public string PhoneNumber { get; set; }
    // public DateTime CreatedAt { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public Cart Cart { get; set; }
}

