
using Bank.Data;
using Bank.Interfaces;
using Bank.Models;
using Bank.Repositories;
using Bank.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Bank
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();
            builder.Services.AddStackExchangeRedisCache(redisOptions =>
            {
                   redisOptions.Configuration = builder.Configuration
                                    .GetConnectionString("Redis"); 
            });
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
      
                .AddApiEndpoints();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration).CreateLogger();

           // builder.Host.UseSerilog();


            //builder.Logging.AddJsonConsole(options =>
            //{
            //    options.JsonWriterOptions = new()
            //    {
            //        Indented = true
            //    };
            //});

            var app = builder.Build();


            app.MapIdentityApi<Models.AppUser>(); 

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           // app.UseSerilogRequestLogging();

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


            app.Run();
        }
    }
}
