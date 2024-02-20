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
                AccountId = accountModel.AccountId,
                FullName = accountModel.AppUser.FullName,
                Balance = accountModel.Balance
            };
        }

    }
}
