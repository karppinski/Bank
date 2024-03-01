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
        public TokenService(IAccountRepository accountRepo, ITransactionRepository transactionRepo)
                                   
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
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
