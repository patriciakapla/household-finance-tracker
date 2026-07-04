
namespace FinanceTracker.Api.Features.Transactions
{
    public class Transaction
    {
        public Guid Id {get; set;}
        public Guid UserId {get; set;}
        public string Description {get; set;} = string.Empty;
        public decimal Amount {get; set;}
        public TransactionType Type {get; set;} 
        public DateTimeOffset CreatedAt {get; set;}
    }
}