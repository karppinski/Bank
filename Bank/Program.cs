
using Bank.Data;
using Bank.Interfaces;
using Bank.Models;
using Bank.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<DataContext>()
                .AddApiEndpoints();

  
            var app = builder.Build();

            app.MapIdentityApi<Models.AppUser>(); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
