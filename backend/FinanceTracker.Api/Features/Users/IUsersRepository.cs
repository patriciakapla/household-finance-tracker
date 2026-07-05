namespace FinanceTracker.Api.Features.Users
{
    public interface IUsersRepository
    {
        Task<User> CreateAsync(CreateUserRequest user);
        Task<User?> DeleteAsync(Guid id);
        Task<IEnumerable<UserDto>> ListAsync();
    }
}
