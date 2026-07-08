
using FinanceTracker.Api.Features.Users;

using Microsoft.AspNetCore.Mvc;

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
            DateTime today = DateTime.Today;

            DateTime max = today.AddYears(-18);


            var user = await _usersRepository.GetByIdAsync(transaction.UserId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.BirthDate > max && transaction.Type == TransactionType.revenue)
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> List()
        {
            var transactions = await _transactionsRepository.ListAsync();

            return Ok(new { Data = transactions });
        }

        [HttpGet("report")]
        public async Task<ActionResult<IEnumerable<TransactionsReportDto>>> GenerateReport()
        {
            var report = await _transactionsRepository.GenerateReportAsync();

            return Ok(new { Data = report });
        }
    }
}