using App.DTO.Account;
using App.Models;

namespace App.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUseriDto(this User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Username = userModel.UserName,
                Email = userModel.Email
            };

        }


    }
}

