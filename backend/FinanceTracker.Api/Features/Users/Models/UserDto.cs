namespace FinanceTracker.Api.Features.Users
{
    public class UserDto
    {
        public Guid Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public DateTime BirthDate {get; set;}
    }
}
