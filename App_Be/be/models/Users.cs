using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string Gender { set; get; }
    public string FullName { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; } = DateTime.Now;
    public string ProfilePictureUrl { get; set; }
    public string MoreInfor { set; get; } = string.Empty;



    [ForeignKey("Address")]
    public string AddressID { get; set; }
    public Address Address { get; set; }



    public virtual ICollection<Review> Reivews { get; set; } = new List<Review>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

}

