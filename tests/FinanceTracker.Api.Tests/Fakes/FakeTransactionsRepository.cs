using FinanceTracker.Api.Features.Transactions;

namespace FinanceTracker.Api.Tests.Fakes;

public class FakeTransactionsRepository : ITransactionsRepository
{
    private readonly IEnumerable<Transaction> _transactionList;
    private readonly IEnumerable<TransactionsReportDto> _transactionsReport;

    public FakeTransactionsRepository(
        IEnumerable<Transaction>? transactionList = null,
        IEnumerable<TransactionsReportDto>? transactionsReport = null
    )
    {
        _transactionList = transactionList ?? [];
        _transactionsReport = transactionsReport ?? [];
    }

    public Task<Transaction> CreateAsync(CreateTransactionRequest transaction)
    {
        return Task.FromResult(new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = transaction.UserId,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            CreatedAt = DateTimeOffset.UtcNow
        });
    }

    public Task<IEnumerable<Transaction>> ListAsync()
    {
        return Task.FromResult(_transactionList);
    }
    public Task<IEnumerable<TransactionsReportDto>> GenerateReportAsync()
    {
        return Task.FromResult(_transactionsReport);
    }
}

