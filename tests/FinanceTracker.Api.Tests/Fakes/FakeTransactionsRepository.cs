using FinanceTracker.Api.Features.Transactions;

namespace FinanceTracker.Api.Tests.Fakes;

public class FakeTransactionsRepository : ITransactionsRepository
{
    private readonly IEnumerable<TransactionDto> _transactionList;
    private readonly IEnumerable<TransactionsReportDto> _transactionsReport;
    private readonly ReportTotalDto _reportTotal;

    public FakeTransactionsRepository(
        IEnumerable<TransactionDto>? transactionList = null,
        IEnumerable<TransactionsReportDto>? transactionsReport = null,
        ReportTotalDto? reportTotal = null
    )
    {
        _transactionList = transactionList ?? [];
        _transactionsReport = transactionsReport ?? [];
        _reportTotal = reportTotal ?? new ReportTotalDto();
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

    public Task<IEnumerable<TransactionDto>> ListAsync()
    {
        return Task.FromResult(_transactionList);
    }

    public Task<IEnumerable<TransactionsReportDto>> GenerateReportAsync()
    {
        return Task.FromResult(_transactionsReport);
    }

    public Task<ReportTotalDto> GenerateTotalAsync()
    {
        return Task.FromResult(_reportTotal);
    }
}
