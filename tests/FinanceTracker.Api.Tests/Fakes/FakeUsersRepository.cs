using FinanceTracker.Api.Features.Users;

namespace FinanceTracker.Api.Tests.Fakes;

public class FakeUsersRepository : IUsersRepository
{
    private readonly User? _user;
    private readonly User? _userToDelete;
    private readonly IEnumerable<UserDto> _usersToList;

    public FakeUsersRepository(
        User? user = null,
        User? userToDelete = null,
        IEnumerable<UserDto>? usersToList = null)
    {
        _user = user;
        _userToDelete = userToDelete;
        _usersToList = usersToList ?? Enumerable.Empty<UserDto>();
    }

    public Task<User> CreateAsync(CreateUserRequest user)
    {
        return Task.FromResult(new User
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Age = user.Age,
            Active = true
        });
    }

    public Task<User?> DeleteAsync(Guid id)
    {
        return Task.FromResult(_userToDelete);
    }

    public Task<IEnumerable<UserDto>> ListAsync()
    {
        return Task.FromResult(_usersToList);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_user);
    }
}
