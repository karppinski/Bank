using Bank.Models;

namespace Bank.Dtos.User
{
    public class UserDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public List<Models.Account> Accounts { get; set; } = new List<Models.Account>();
    }
}
