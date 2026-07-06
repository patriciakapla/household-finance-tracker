using FinanceTracker.Api.Features.Transactions;

namespace FinanceTracker.Api.Tests.Fakes;

public class FakeTransactionsRepository : ITransactionsRepository
{
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
}
