using Azure;
using Bank.Data;
using Bank.Dtos.User;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Bank.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITransactionRepository _transactionRepo;
        private readonly HttpClient _httpClient;

        public UserRepository(DataContext context, IAccountRepository accountRepo, ITransactionRepository transactionRepo,
            UserManager<AppUser> userManager, HttpClient httpClient )
        {
            _context = context;
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _userManager = userManager;
            _httpClient = httpClient;
        }

        public async Task<AppUser> CreateUser(CreateUserDto userDto)
        {
            var newUser = new AppUser
            {
                FullName = userDto.FullName,
                DateOfBirth = userDto.DateOfBirth,
                UserName = userDto.FullName.Replace(" ", ""),
                Email = userDto.Email,
            };

           if (await _context.Users.AnyAsync(user => user.Email == newUser.Email) ||
                await _context.Users.AnyAsync(user => user.UserName == newUser.UserName))
            {
                throw new Exception("Email or login already in use");
            }

            await _userManager.CreateAsync(newUser, userDto.Password);

            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<AppUser> DeleteUser(string id)
        {
            var user = await GetUserById(id);
            if(user == null)
            {
                throw new Exception("User not found");
            }

            var tempUserToReturn = user;

            await _transactionRepo.DeleteTransactionsByUserId(id);
            await _accountRepo.DeleteAccountsForUserId(id);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return tempUserToReturn;

        }

        public async Task<AppUser> GetUserById(string id)
        {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if(user == null)
            {
                throw new Exception("User not found. ");
            }

            return user;
        }

        public async Task<List<AppUser>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var loginRequest = await _httpClient.PostAsJsonAsync("https://localhost:7059/login", loginDto);
            if (loginRequest.IsSuccessStatusCode)
            {
                var responseContent = await loginRequest.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new HttpRequestException($"Login failed with status code: {loginRequest.StatusCode}", null, loginRequest.StatusCode);
            }
        
        }

        public async Task<AppUser> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found. ");
            }
            user.FullName = updateUserDto.FullName;

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
