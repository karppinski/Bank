﻿using Bank.Data;
using Bank.Dtos.User;
using Bank.Interfaces;
using Bank.Mappers;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITransactionRepository _transactionRepo;

        public UserRepository(DataContext context, IAccountRepository accountRepo, ITransactionRepository transactionRepo,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _userManager = userManager;
            _signInManager = signInManager;
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
