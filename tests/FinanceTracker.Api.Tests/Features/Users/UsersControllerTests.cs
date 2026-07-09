using FinanceTracker.Api.Features.Users;
using FinanceTracker.Api.Tests.Fakes;

using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Tests.Features.Users;

public class UsersControllerTests
{
    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedUser()
    {
        var repository = new FakeUsersRepository();
        var controller = new UsersController(repository);
        var request = new CreateUserRequest
        {
            Name = "Buffy Summers",
            BirthDate = DateTime.Today.AddYears(-18)
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var response = Assert.IsType<UserDto>(createdResult.Value);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Buffy Summers", response.Name);
        Assert.Equal(request.BirthDate, response.BirthDate);
        Assert.Equal($"/users/{response.Id}", createdResult.Location);
    }

    [Fact]
    public async Task Delete_WithExistingUser_ReturnsNoContent()
    {
        var userId = Guid.NewGuid();
        var repository = new FakeUsersRepository(
            userToDelete: new User
            {
                Id = userId,
                Name = "Buffy Summers",
                BirthDate = DateTime.Today.AddYears(-18),
                Active = false
            });
        var controller = new UsersController(repository);

        var result = await controller.Delete(userId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WithMissingUser_ReturnsNotFound()
    {
        var repository = new FakeUsersRepository();
        var controller = new UsersController(repository);

        var result = await controller.Delete(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsUsers()
    {
        var users = new[]
        {
            new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Buffy Summers",
                BirthDate = DateTime.Today.AddYears(-18)
            },
            new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Rupert Giles",
                BirthDate = DateTime.Today.AddYears(-43)
            }
        };
        var repository = new FakeUsersRepository(usersToList: users);
        var controller = new UsersController(repository);

        var result = await controller.ListAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<UserDto>>(GetData(okResult.Value));
        var responseList = response.ToList();

        Assert.Equal(2, responseList.Count);
        Assert.Equal("Buffy Summers", responseList[0].Name);
        Assert.Equal(users[0].BirthDate, responseList[0].BirthDate);
        Assert.Equal("Rupert Giles", responseList[1].Name);
        Assert.Equal(users[1].BirthDate, responseList[1].BirthDate);
    }

    private static object? GetData(object? value)
    {
        Assert.NotNull(value);

        return value.GetType().GetProperty("Data")?.GetValue(value);
    }
}
