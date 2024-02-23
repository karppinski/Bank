using Bank.Dtos.User;
using Bank.Interfaces;
using Bank.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;

        public UserController(IUserRepository userRepo, ITokenService tokenService,
            ITransactionRepository transactionRepo, IAccountRepository accountRepo)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _transactionRepo = transactionRepo;
            _accountRepo = accountRepo;
        }


        [HttpGet("getAll")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetUsers();

            return Ok(users);
        }

        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var canAccess = _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only Your account");
            }

            var user =await _userRepo.GetUserById(id);

            var userDto = user.ToUserDto();

            return Ok(userDto);

        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var newUser = await _userRepo.CreateUser(userDto);

            return Ok(newUser);

        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto userDto)
        {
            var canAccess = _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You canaccess only your account!");
            }

            var user = await _userRepo.GetUserById(id);
            user.FullName = userDto.FullName;

            return Ok(userDto);

        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var canAccess = _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You canaccess only your account!");
            }

            await _transactionRepo.DeleteTransactionsByUserId(id);
            await _accountRepo.DeleteAccountsForUserId(id);
            await _userRepo.DeleteUser(id);

            return NoContent();

        }
    }
}
