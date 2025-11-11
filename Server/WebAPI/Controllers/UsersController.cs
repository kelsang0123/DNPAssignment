using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

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
        public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
            User user = new(request.UserName, request.Password);
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.Username
            };
            return Created($"/Users/{dto.Id}", dto);
        }
        private async Task VerifyUserNameIsAvailableAsync(string userName)
        {
            bool exists = userRepo.GetMany()
            .Any(u => u.Username.ToLower().Equals(userName.ToLower()));
            if (exists)
            {
                throw new InvalidOperationException(
              $"The username '{userName}' is already taken.");
            }
        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<User>> GetSingleUser([FromRoute] int id)
        {
            User user = await userRepo.GetSingleAsync(id);
            return Ok(user);
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers(
            [FromQuery] string? userNameContains = null)
        {
            IQueryable<User> users = userRepo.GetMany()
            .Where(
            u => userNameContains == null ||
                   u.Username.ToLower().Contains(userNameContains.ToLower())
            );
            return Ok(users);
        }
        [HttpGet("{userId:int}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsForUser(
             [FromRoute] int userId,
             [FromServices] IPostRepository postRepo)
        {
            List<Post> posts = postRepo.GetMany()
            .Where(p => p.UserId == userId)
            .ToList();
            return Ok(posts);
        }
        [HttpGet("{userId:int}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsForUser(
           [FromRoute] int userId,
           [FromServices] ICommentRepository commentRepo)
        {
            List<Comment> comments = commentRepo.GetMany()
            .Where(c => c.UserId == userId)
            .ToList();
            return Ok(comments);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(
                 [FromRoute] int id,
                 [FromBody] UpdateUserDto request)
        {
            User userToUpdate = await userRepo.GetSingleAsync(id);
            userToUpdate.Username = request.UserName;
            userToUpdate.Password = request.Password;
            await userRepo.UpdateAsync(userToUpdate);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IResult> DeleteUser(
            [FromRoute] int id
        )
        {
            await userRepo.DeleteAsync(id);
            return Results.NoContent();
        }
        
        }
    }