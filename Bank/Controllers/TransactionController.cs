﻿using Bank.Dtos.Transaction;
using Bank.Interfaces;
using Bank.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController :ControllerBase
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly ITokenService _tokenService;
        public TransactionController(ITransactionRepository transactionRepo, ITokenService tokenService)
        {
            _transactionRepo = transactionRepo;
            _tokenService = tokenService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionRepo.GetAllTransactions();

            return Ok(transactions);
        }

        [HttpGet("GetAllForAccount/{id}")]
        public async Task<IActionResult> GetAllForAccount(int id)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var transactions = await _transactionRepo.GetAllTransactionsForAnAccount(id);

            var transactionsDto = transactions.Select(t => t.ToTransactionDto());

            return Ok(transactionsDto);
        }

        [HttpGet("GetAllForAnUser/{id}")]
        public async Task<IActionResult> GetAllForAnUser(string id)
        {
            var canAccess =  _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var userId = _tokenService.GetUserIdFromClaims(User);

            var transactions = await _transactionRepo.GetAllTransactionsForAnUser(userId);

            var transactionsDto = transactions.Select(t => t.ToTransactionDto());

            return Ok(transactionsDto);
        }

        [HttpPost("depoist")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto depositDto)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, depositDto.AccountId);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var transaction = await _transactionRepo.Deposit(depositDto);

            var transactionDto = transaction.ToTransactionDto();

            return Ok(transactionDto);
        }  
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto withdrawDto)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, withdrawDto.AccountId);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var transaction = await _transactionRepo.Withdraw(withdrawDto);

            var transactionDto = transaction.ToTransactionDto();

            return Ok(transactionDto);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto transferDto)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, transferDto.FromAccountId);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var transfer = await _transactionRepo.Transfer(transferDto);

            var transactionDto =  transfer.ToTransactionDtos();

            return Ok(transactionDto);
        }

        [HttpGet("GetTransactionById/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {

            var canAccess = await _tokenService.UserCanAccesTransaction(User, id);

            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            var transaction = await _transactionRepo.GetTransactionById(id);

            var transactionDto = transaction.ToTransactionDto();
            return Ok(transactionDto);
        }

        [HttpDelete("deleteForAccount/{id}")]
        public async Task<IActionResult> DeleteTransactionFroAccount(int id)
        {
            var canAccess = await _tokenService.UserCanAccessAccount(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            await _transactionRepo.DeleteTransactionsByAccountId(id);

            return NoContent();

        }

        [HttpDelete("deleteForUser/{id}")]
        public async Task<IActionResult> DeleteTransactionsForAnUser(string id)
        {
            var canAccess = _tokenService.UserCanAccessUser(User, id);
            if (!canAccess)
            {
                return Unauthorized("You can access only your account! ");
            }

            await _transactionRepo.DeleteTransactionsByUserId(id);

            return NoContent();
        }

    }
}
