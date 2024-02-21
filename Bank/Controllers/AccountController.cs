using Bank.Interfaces;
using Bank.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace Bank.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpGet("GetAllForAnAdmin")]
        [Authorize(Roles ="Admin")]
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
        [HttpGet("GetAllForAnUser")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> GetAllForAnUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User id not found in the token");
            }

            var userId = userIdClaim.Value;

            var accounts = await _accountRepo.GetAccountsForAnUser(userId);

            var accountDto = accounts.Select(a => a.ToAccountDto());

            return Ok(accountDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAccountForAnUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null) 
            {
                 return Unauthorized("User id not found in the token");
            }
            
            var userId = userIdClaim.Value;

            var account = await _accountRepo.GetUserAccountWithId(userId, id);

            if(account == null)
            {
                return Unauthorized("You can access only your account");
            }

            var accountDto = account.ToAccountDto();

            return Ok(accountDto);



        }
    }
}
