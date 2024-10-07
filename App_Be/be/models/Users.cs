using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

}

