namespace Bank.Dtos.User
{
    public class CreateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
