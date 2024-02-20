using Bank.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    public class TransactionController :ControllerBase
    {
        private readonly ITransactionRepository _transactionRepo;

    }
}
