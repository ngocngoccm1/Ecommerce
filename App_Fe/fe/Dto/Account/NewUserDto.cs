namespace App.DTO.Account
{
    public class NewUserDto
    {
        public string UserName { set; get; } = string.Empty;
        public string Email { set; get; } = string.Empty;
        public string Token { set; get; } = string.Empty;

    }
}