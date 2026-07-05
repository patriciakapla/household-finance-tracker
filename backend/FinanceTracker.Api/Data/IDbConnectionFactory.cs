using System.Data;

namespace FinanceTracker.Api.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
