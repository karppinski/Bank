using Bank.Interfaces;
using Bank.Mappers;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ITokenService _tokenService;
        private readonly ITransactionRepository _transactionRepo;

        public AccountController(IAccountRepository accountRepo, ITokenService tokenService, ITransactionRepository transactionRepo)
        {
            _accountRepo = accountRepo;
            _tokenService = tokenService;
            _transactionRepo = transactionRepo;
        }

        [HttpGet("GetAllForAnAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllForAnAdmin()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var accounts = await _accountRepo.GetAccounts();

            var accountDto = accounts.Select(a => a.ToAccountDto());

            return Ok(accountDto);
        }
        [HttpGet("UserAccounts/{id}")]
        public async Task<IActionResult> GetAllForAnUser(string id)
        {

            var canAccess =  _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }
            var userId =  _tokenService.GetUserIdFromClaims(User);

            var accounts = await _accountRepo.GetAccountsForAnUser(userId);

            var accountDto = accounts.Select(a => a.ToAccountDto());

            return Ok(accountDto);
        }

        [HttpGet("account/{id}")]
            public async Task<IActionResult> GetAccountById(int id)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var account = await _accountRepo.GetAccountWithId(id);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var accountDto = account.ToAccountDto();

            return Ok(accountDto);
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
          

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User id not found in the token");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = userIdClaim.Value;

            var newAccount = await _accountRepo.CreateAccount(userId);
            var accountDto = newAccount.ToAccountDto();

            return Ok(accountDto);
        }

        [HttpDelete("deleteOne/{id}")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            await _transactionRepo.DeleteTransactionsByAccountId(id);
            await _accountRepo.DeleteAccountById(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("deleteAll/{id}")]
        public async Task<IActionResult> DeleteAllAccounts(string id)
        {
            var canAccess =  _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _tokenService.GetUserIdFromClaims(User);

            await _transactionRepo.DeleteTransactionsByUserId(userId);
            await _accountRepo.DeleteAccountsForUserId(userId);

        
            return NoContent();
        }

    }
}
