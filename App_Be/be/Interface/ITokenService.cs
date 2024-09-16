namespace App.Interface
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}