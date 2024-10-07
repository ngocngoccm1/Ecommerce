namespace DTO.Account
{
    public class UserDto
    {
        public string? Id { set; get; }
        public string? Username { set; get; }
        public string? Email { set; get; }

        public List<string>? Roles { set; get; }
    }
}