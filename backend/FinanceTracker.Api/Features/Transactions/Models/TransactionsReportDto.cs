
namespace FinanceTracker.Api.Features.Transactions
{
    public class TransactionsReportDto
    {
        public string Username {get; set;} = string.Empty;
        public double Revenue {get; set;} 
        public double Expenses {get; set;} 
        public double Balance {get; set;} 
    }
}