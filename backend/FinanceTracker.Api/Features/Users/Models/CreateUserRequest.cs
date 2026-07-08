
namespace FinanceTracker.Api.Features.Users
{
    public class CreateUserRequest
    {
        public string Name {get; set;} = string.Empty;
        public DateTime BirthDate {get; set;}
    }
}
