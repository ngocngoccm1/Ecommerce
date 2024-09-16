using System.ComponentModel.DataAnnotations;

namespace App.DTO.Account
{
    public class LoginDto
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
    }
}