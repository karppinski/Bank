using Bank.Interfaces;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bank.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IAccountRepository accountRepo, ITransactionRepository transactionRepo,
                                    IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _configuration = configuration;
            _userManager = userManager;
        }

        //public async Task<string> CreateToken(AppUser user)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var claims = new List<Claim>
        //{
        //    new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
        //};

        //    // Optionally add roles as claims
        //    var roles = await _userManager.GetRolesAsync(user);
        //    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = creds
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}

        public string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

            public async Task<bool> UserCanAccessAccount(ClaimsPrincipal user, int id)
        {
            var userId = GetUserIdFromClaims(user);
            var account = await _accountRepo.GetAccountWithId(id);

            const string adminId = "fa55ceee-04bf-4164-a97d-b3dfdccc5fce";
            if(account == null)
            {
                return false;
            }
       
            return account.AppUserId == userId || userId == adminId;
        }
        public bool UserCanAccessUser(ClaimsPrincipal user, string userIdFromRoute)
        {
            var userId =  GetUserIdFromClaims(user);

            const string adminId = "fa55ceee-04bf-4164-a97d-b3dfdccc5fce";

          
            return userId == userIdFromRoute || userId == adminId;
        }
        public async Task<bool> UserCanAccesTransaction(ClaimsPrincipal user, int id)
        {
            var userId = GetUserIdFromClaims(user);
            var transaction = await _transactionRepo.GetTransactionById(id);

            const string adminId = "fa55ceee-04bf-4164-a97d-b3dfdccc5fce";
            if (transaction == null)
            {
                return false;
            }
           
            return transaction.Account.AppUserId == userId || userId == adminId;
        }

    }
}
