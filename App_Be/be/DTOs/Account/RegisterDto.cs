using System.ComponentModel.DataAnnotations;

namespace App.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        public string Username { set; get; }
        [Required]
        [EmailAddress]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }



    }
}