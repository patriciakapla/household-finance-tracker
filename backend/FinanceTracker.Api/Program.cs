using FinanceTracker.Api.Data;
using FinanceTracker.Api.Features.Users;
using FinanceTracker.Api.Features.Transactions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new
{
    Name = "Household Finance Tracker API",
    Status = "Running"
}));

app.MapControllers();

app.Run();
