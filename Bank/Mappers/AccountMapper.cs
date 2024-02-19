using Bank.Dtos.Account;
using Bank.Models;

namespace Bank.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this Account accountModel)
        {
            return new AccountDto
            {
                FullName = accountModel.FullName,
                Balance = accountModel.Balance
            };
        }

    }
}
