using Dapper;
using FinanceTracker.Api.Data;

namespace FinanceTracker.Api.Features.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsersRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> CreateAsync(CreateUserRequest user)
        {

            const string sql = """
                INSERT INTO users ("name", age)
                VALUES (@Name, @Age)
                RETURNING id, "name", age, active;
                """;

            using var connection = _connectionFactory.CreateConnection();


            return await connection.QuerySingleAsync<User>(sql, user);
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            const string sql = """
                UPDATE users
                SET active = false
                WHERE id = @Id
                AND active = true
                RETURNING id, "name", age, active;
                """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }
    }
}
