
namespace FinanceTracker.Api.Features.Transactions
{
    public class TransactionsDto
    {
        public string Username {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public decimal Amount {get; set;}
        public TransactionType Type {get; set;} 
    }
}