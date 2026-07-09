using FinanceTracker.Api.Features.Transactions;
using FinanceTracker.Api.Features.Users;
using FinanceTracker.Api.Tests.Fakes;

using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Tests.Features.Transactions;

public class TransactionsControllerTests
{
    [Fact]
    public async Task Create_WithAdultUser_ReturnsCreated()
    {
        var userId = Guid.NewGuid();
        var controller = new TransactionsController(
            new FakeTransactionsRepository(),
            new FakeUsersRepository(user: RupertGiles(userId)));
        var request = new CreateTransactionRequest
        {
            UserId = userId,
            Description = "Research",
            Amount = 125.9m,
            Type = TransactionType.revenue
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var response = Assert.IsType<TransactionDto>(createdResult.Value);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(userId, response.UserId);
        Assert.Equal("Research", response.Description);
        Assert.Equal(125.9m, response.Amount);
        Assert.Equal(TransactionType.revenue, response.Type);
        Assert.Equal($"/transactions/{response.Id}", createdResult.Location);
    }

    [Fact]
    public async Task Create_WithMissingUser_ReturnsNotFound()
    {
        var controller = new TransactionsController(
            new FakeTransactionsRepository(),
            new FakeUsersRepository());

        var result = await controller.Create(ExpenseRequest(Guid.NewGuid()));

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_WithUnderageUserAndRevenue_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        var controller = new TransactionsController(
            new FakeTransactionsRepository(),
            new FakeUsersRepository(user: BuffySummers(userId)));

        var result = await controller.Create(new CreateTransactionRequest
        {
            UserId = userId,
            Description = "Vampire slaying",
            Amount = 500m,
            Type = TransactionType.revenue
        });

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task Create_WithUnderageUserAndExpense_ReturnsCreated()
    {
        var userId = Guid.NewGuid();
        var controller = new TransactionsController(
            new FakeTransactionsRepository(),
            new FakeUsersRepository(user: BuffySummers(userId)));

        var result = await controller.Create(ExpenseRequest(userId));

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var response = Assert.IsType<TransactionDto>(createdResult.Value);

        Assert.Equal(userId, response.UserId);
        Assert.Equal(TransactionType.expense, response.Type);
    }

    private static CreateTransactionRequest ExpenseRequest(Guid userId)
    {
        return new CreateTransactionRequest
        {
            UserId = userId,
            Description = "Research expenses",
            Amount = 42.75m,
            Type = TransactionType.expense
        };
    }

    private static User BuffySummers(Guid id)
    {
        return new User
        {
            Id = id,
            Name = "Buffy Summers",
            BirthDate = DateTime.Today.AddYears(-16),
            Active = true
        };
    }

    private static User RupertGiles(Guid id)
    {
        return new User
        {
            Id = id,
            Name = "Rupert Giles",
            BirthDate = DateTime.Today.AddYears(-43),
            Active = true
        };
    }
}
