using Dapper;
using FinanceTracker.Api.Data;

namespace FinanceTracker.Api.Features.Transactions
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TransactionsRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Transaction> CreateAsync(CreateTransactionRequest transaction)
        {
        
            const string sql = """
                INSERT INTO transactions 
                (user_id, "description", amount, "type")
                VALUES (@UserId, @Description, @Amount, @Type::transaction_type)
                RETURNING id, user_id AS UserId, "description", amount, "type", created_at AS CreatedAt;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleAsync<Transaction>(sql, new 
            {
                transaction.UserId,
                transaction.Description,
                transaction.Amount,
                Type = transaction.Type.ToString().ToLowerInvariant()
            });
        }

    }
}
