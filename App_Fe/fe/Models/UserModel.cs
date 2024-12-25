using System;

namespace Models
{
    public class UserModel
    {
        public string Id { set; get; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string MoreInfo { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
