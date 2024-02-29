using Bank.Dtos.User;
using Bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetUsers();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> CreateUser(CreateUserDto userDto);
        Task<AppUser> UpdateUser(string id, UpdateUserDto updateUserDto);
        Task<AppUser> DeleteUser(string id);
        Task<string> Login(LoginDto loginDto);

    }
}
