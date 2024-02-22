using Bank.Interfaces;
using System.Security.Claims;

namespace Bank.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAccountRepository _accountRepo;
        public TokenService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }
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

    }
}
