

namespace FinanceTracker.Api.Features.Transactions
{
    public interface ITransactionsRepository
    {
        Task<Transaction> CreateAsync(CreateTransactionRequest transaction);


    }
}