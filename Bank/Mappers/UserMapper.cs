using Bank.Dtos.User;
using Bank.Models;

namespace Bank.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this AppUser userModel)
        {
            return new UserDto
            {
                FullName = userModel.FullName,
                DateOfBirth = userModel.DateOfBirth,
                Accounts = userModel.Accounts
            };
        }
    }
}
