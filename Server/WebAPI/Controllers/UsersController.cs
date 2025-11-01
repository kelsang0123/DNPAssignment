using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using RepositoryContracts;
using DTOs;
using FileRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepo;

        public UsersController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] CreateUserDTO request)
        {
            try
            {
                await VerifyUserNameIsAvailableAsync(request.UserName);
                User user = new(request.UserName, request.Password);
                User created = await userRepo.AddAsync(user);
                UserDto dto = new()
                {
                    Id = created.Id,
                    UserName = created.UserName
                };
                return Created($"/users/{dto.Id}", created);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{Id:int}")]
        public async Task<IResult> GetSingleUser([FromRoute] int id)
        {
            User user = await userRepo.GetSingleAsync(id);
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return Results.Ok(userDto);
        }
        [HttpGet]
        public async Task<IResult> GetUsers(
            [FromQuery] string? UserName = null,
            [FromQuery] int? Id = null)
        {
            List<UserDto> users = userRepo.GetMany().Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();
            //List<UserDto> userDtos = users.Select(u)
            return Results.Ok(users);
        }
        [HttpPatch("{Id:int}")]
        public async Task<IResult> UpdateUser(
                 [FromRoute] int id,
                 [FromBody] UpdateUserDto request)
        {
            User userToUpdate = await userRepo.GetSingleAsync(id);
            userToUpdate.UserName = request.UserName;
            await userRepo.UpdateAsync(userToUpdate);
            return Results.NoContent();
        }
        [HttpDelete("{Id:int}")]
        public async Task<IResult> DeleteUser(
            [FromRoute] int id
        )
        {
            await userRepo.DeleteAsync(id);
            return Results.NoContent();
        }
        private async Task VerifyUserNameIsAvailableAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(userName));
            }
            //Get all users from the repository
            List<UserDto> users = userRepo.GetMany().Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();
            //CheckIfUsernameAlreadyExist(case-sensitive)
            bool exists = users.Any(u =>
            u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                throw new InvalidOperationException(
              $"The username '{userName}' is already taken.");
            }
        }
    }
}