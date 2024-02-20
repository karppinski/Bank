using Bank.Interfaces;
using Bank.Mappers;
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

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpGet("GetAllForAdmin")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var accounts = await _accountRepo.GetAccounts();

            var accountDto = accounts.Select(a => a.ToAccountDto());

            return Ok(accountDto);
        }      
        [HttpGet("GetAllForUser")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> GetAllForUser()
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
    }
}
