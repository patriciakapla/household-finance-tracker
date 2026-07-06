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
            Age = 16
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var response = Assert.IsType<UserDto>(createdResult.Value);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Buffy Summers", response.Name);
        Assert.Equal(16, response.Age);
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
                Age = 16,
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
                Age = 16
            },
            new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Rupert Giles",
                Age = 43
            }
        };
        var repository = new FakeUsersRepository(usersToList: users);
        var controller = new UsersController(repository);

        var result = await controller.ListAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        var responseList = response.ToList();

        Assert.Equal(2, responseList.Count);
        Assert.Equal("Buffy Summers", responseList[0].Name);
        Assert.Equal(16, responseList[0].Age);
        Assert.Equal("Rupert Giles", responseList[1].Name);
        Assert.Equal(43, responseList[1].Age);
    }

}
