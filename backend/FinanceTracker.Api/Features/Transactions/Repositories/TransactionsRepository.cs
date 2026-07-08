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

        public async Task<IEnumerable<TransactionDto>> ListAsync()
        {
            const string sql = """
                SELECT 
                t.id, 
                t.user_id AS UserId,
                t."description", 
                t.amount, 
                t."type", 
                t.created_at AS CreatedAt,
                u.name AS UserName
                FROM transactions t
                LEFT JOIN users u ON t.user_id = u.id
                WHERE u.active = true
                ORDER BY created_at;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<TransactionDto>(sql);
        }

        public async Task<IEnumerable<TransactionsReportDto>> GenerateReportAsync()
        {
            const string sql = """
                SELECT
                u.id AS UserId,
                u.name AS Username,
                SUM(CASE WHEN t.type = 'revenue' THEN t.amount ELSE 0 END) AS Revenue,
                SUM(CASE WHEN t.type = 'expense' THEN t.amount ELSE 0 END) AS Expenses,
                SUM(CASE WHEN t.type = 'revenue' THEN t.amount ELSE 0 END) -
                SUM(CASE WHEN t.type = 'expense' THEN t.amount ELSE 0 END) AS Balance
                FROM users u
                LEFT JOIN transactions t ON u.id = t.user_id
                WHERE u.active = true
                GROUP BY u.id;
            """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<TransactionsReportDto>(sql);
        }
    }
}