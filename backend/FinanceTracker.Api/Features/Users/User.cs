
namespace FinanceTracker.Api.Features.Users
{
    public class User
    {
       public Guid Id {get; set;}
       public string Name {get; set;} = string.Empty;
       public int Age {get; set;}
       public bool Active {get;set;}
    }
}