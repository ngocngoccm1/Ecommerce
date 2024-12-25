using System.ComponentModel.DataAnnotations;

namespace App.DTO.Account
{
    public class UserUpdate
    {

        public string Gender { get; set; }
        public string FullName { get; set; }
        public IFormFile ProfilePictureUrl { get; set; }
        public string MoreInfor { get; set; }

        // public Address Address { get; set; } = new Address();
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // public Address Address { get; set; } = new Address();
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }


    }
}