// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace App.Models;

// public class UserInfor
// {
//     [Key]
//     public string Id { set; get; }
//     public string Gender { set; get; }
//     public string FullName { get; set; } = string.Empty;
//     public DateTime JoinedDate { get; set; } = DateTime.Now;
//     public string ProfilePictureUrl { get; set; }
//     public string MoreInfor { set; get; } = string.Empty;
//     [ForeignKey("User")]
//     public string UserId { get; set; }
//     public virtual User User { get; set; }

//     [ForeignKey("Address")]
//     public string AddressID { get; set; }
//     public Address Address { get; set; }
// }
