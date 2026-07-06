
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.Api.Features.Users;

namespace FinanceTracker.Api.Features.Transactions
{
    [ApiController]
    [Route("transactions")]
    public class TransactionsController : ControllerBase

    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IUsersRepository _usersRepository;

        public TransactionsController(
            ITransactionsRepository transactionRepository,
            IUsersRepository usersRepository)
        {
            _transactionsRepository = transactionRepository;
            _usersRepository = usersRepository;
        }

    [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionRequest transaction)
        {
            var user = await _usersRepository.GetByIdAsync(transaction.UserId);

            if (user == null)
            {
                return NotFound();
            }
        
            if (user.Age < 18 && transaction.Type == TransactionType.Revenue)
            {
                return BadRequest();
            }

            var createdTransaction = await _transactionsRepository.CreateAsync(transaction);

            var response = new TransactionDto
            {
                Id = createdTransaction.Id,
                UserId = createdTransaction.UserId,
                Description = createdTransaction.Description,
                Amount = createdTransaction.Amount,
                Type = createdTransaction.Type,
                CreatedAt = createdTransaction.CreatedAt
            };

            return Created($"/transactions/{response.Id}", response);

        }
    }

}
