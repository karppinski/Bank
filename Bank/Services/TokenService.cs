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

            if(userId == adminId)
            {
                return true;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return account.AppUserId == userId;
        }
        public async Task<bool> UserCanAccessUser(ClaimsPrincipal user)
        {
            var userId = GetUserIdFromClaims(user);
            var account = await _accountRepo.GetAccountWithId(id);

            const string adminId = "fa55ceee-04bf-4164-a97d-b3dfdccc5fce";

            if(userId == adminId)
            {
                return true;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return account.AppUserId == userId;
        }

    }
}
