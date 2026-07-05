using Dapper;
using FinanceTracker.Api.Data;

namespace FinanceTracker.Api.Features.Users
{
    public class UsersRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsersRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> CreateAsync(CreateUserRequest user)
        {
            // The SQL inserts only fields controlled by the client.
            // id and active are filled by PostgreSQL defaults defined in init.sql.
            const string sql = """
                INSERT INTO users ("name", age)
                VALUES (@Name, @Age)
                RETURNING id, "name", age, active;
                """;

            using var connection = _connectionFactory.CreateConnection();

            // Dapper maps @Name and @Age from the CreateUserRequest properties.
            // QuerySingleAsync returns the row produced by RETURNING.
            return await connection.QuerySingleAsync<User>(sql, user);
        }
    }
}
