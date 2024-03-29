﻿using Bank.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

       public DbSet<Account> Accounts { get; set; }
       public DbSet<Transaction> Transactions { get; set; }

    }
}
