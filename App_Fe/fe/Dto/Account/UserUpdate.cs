public class UserUpdate
{

    public string Gender { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public IFormFile? ProfilePictureUrl { get; set; }
    public string MoreInfor { get; set; } = string.Empty;

    // public Address Address { get; set; } = new Address();
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // public Address Address { get; set; } = new Address();
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;


}