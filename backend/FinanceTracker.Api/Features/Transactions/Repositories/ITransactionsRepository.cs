

namespace FinanceTracker.Api.Features.Transactions
{
    public interface ITransactionsRepository
    {
        Task<Transaction> CreateAsync(CreateTransactionRequest transaction);
        Task<IEnumerable<TransactionDto>> ListAsync();
        Task<IEnumerable<TransactionsReportDto>> GenerateReportAsync();

    }
}