using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Features.Users
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required.");
            }

            if (request.Age < 0)
            {
                return BadRequest("Age cannot be negative.");
            }

            var createdUser = await _usersRepository.CreateAsync(request);

            var response = new UserDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Age = createdUser.Age
            };

            return Created($"/users/{response.Id}", response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedUser = await _usersRepository.DeleteAsync(id);

            if (deletedUser is null)
            {
                return NotFound();
            }

            return NoContent();
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> ListAsync()
        {
            var users = await _usersRepository.ListAsync();

            return Ok(users);
        }
    }
}
