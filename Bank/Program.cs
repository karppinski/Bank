
using Bank.Data;
using Bank.Interfaces;
using Bank.Models;
using Bank.Repositories;
using Bank.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
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

            using(var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "User" };

                foreach (var role in roles) 
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager =
            //        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //    string email = "admin@admin.com";
            //    string password = "P@ssword_111";

            //    if(await userManager.FindByEmailAsync(email) == null)
            //    {
            //        var user = new IdentityUser();
            //        user.UserName = email;
            //        user.Email = email;

            //        await userManager.CreateAsync(user, password);

            //        await userManager.AddToRoleAsync(user, "Admin");
            //    }
            //}

                app.Run();
        }
    }
}
