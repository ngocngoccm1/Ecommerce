
using System.ComponentModel.DataAnnotations;
namespace DTO.Account
{
    public class LoginDto
    {

        [Display(Name = "Tên Đăng nhập")]
        public string Username { set; get; } = string.Empty;
        [Display(Name = "Tên mật khẩu")]

        public string Password { set; get; } = string.Empty;
    }
}