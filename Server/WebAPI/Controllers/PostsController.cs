using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository postRepo;

        public PostsController(IPostRepository postRepo)
        {
            this.postRepo = postRepo;
        }
        [HttpPost]
        public async Task<IResult> AddPost(
          [FromBody] CreatePostDto request,
          [FromServices] IUserRepository userRepo)
        {
            await VerifyAuthorExists(request.AuthorUserId, userRepo);

            Post post = new(request.Title, request.Body, request.AuthorUserId);
            Post created = await postRepo.AddAsync(post);
            return Results.Created($"/posts/{created.Id}", created);
        }

        private async Task VerifyAuthorExists(int userId, IUserRepository userRepo)
        {
            _ = await userRepo.GetSingleAsync(userId);
            // if no user is found, and exception is thrown from the repository
            // the underscore means we don't care about the result. The compiler will optimize it away.
        }

        [HttpPut("{id:int}")]
        public async Task<IResult> UpdatePost(
            [FromRoute] int id,
            [FromBody] UpdatePostDto request)
        {
            Post postToUpdate = await postRepo.GetSingleAsync(id);
            postToUpdate.Title = request.Title;
            postToUpdate.Body = request.Body;

            await postRepo.UpdateAsync(postToUpdate);
            return Results.NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePost([FromRoute] int id)
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IResult> GetSinglePost(
             [FromServices] IUserRepository userRepo,
             [FromServices] ICommentRepository commentRepo,
             [FromRoute] int id,
             [FromQuery] bool includeAuthor,  //
             [FromQuery] bool includeComments)
        {
            Post post = await postRepo.GetSingleAsync(id);
            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
            };
            dto.Author = await IncludeAuthorIfRequested(userRepo, includeAuthor, post.UserId);
            dto.Comments = IncludeCommentsIfRequested(commentRepo, id, includeComments);
            return Results.Ok(dto);
        }
        private static async Task<UserDto?> IncludeAuthorIfRequested(IUserRepository userRepo, bool includeAuthor, int authorId)
        {
            if (!includeAuthor) return null;
            User author = await userRepo.GetSingleAsync(authorId);
            return new UserDto
            {
                Id = author.Id,
                UserName = author.Username
            };
        }
        private static List<CommentDto> IncludeCommentsIfRequested(ICommentRepository commentRepo, int id, bool includeComments)
        {
            if (!includeComments) return [];
            return commentRepo.GetMany()
                .Where(c => c.PostId == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    AuthorUserId = c.UserId,
                })
                .ToList();
        }
        [HttpGet]
        public async Task<IResult> GetPosts([FromQuery] string? titleContains = null, [FromQuery] int? userId = null)
        {
            // This is another filtering approach than in the Users Controller.
            // Either is fine. It's just to show you two different ways.
            IQueryable<Post> queryablePosts = postRepo.GetMany();
            if (titleContains != null)
            {
                queryablePosts = queryablePosts.Where(p => p.Title.Contains(titleContains));
            }

            if (userId != null)
            {
                queryablePosts = queryablePosts.Where(p => p.UserId == userId);
            }

            // using Select() here. It's a simpler way to convert a list of objects to a list of other objects.
            // See the method above, where I convert Comment to CommentDto in "old fashion approach". 
            List<PostDto> posts = queryablePosts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            })
                .ToList();
            return Results.Ok(posts);
        }
        [HttpPost("{postId:int}/comments")]
        public async Task<ActionResult<CommentDto>> AddComment(
            [FromRoute] int postId,
            [FromBody] CreateCommentDto request,
            [FromServices] IUserRepository userRepo,
            [FromServices] ICommentRepository commentRepo)
        {
            await VerifyPostExists(postId);
            await VerifyAuthorExists(request.AuthorUserId, userRepo);

            Comment comment = new(request.Body, request.AuthorUserId, postId);
            Comment created = await commentRepo.AddAsync(comment);
            CommentDto resultDto = new()
            {
                Id = created.Id,
                Body = created.Body,
                AuthorUserId = created.UserId,
                PostId = created.PostId
            };
            return Created($"/Comments/{created.Id}", resultDto);
        }
        private async Task VerifyPostExists(int postId)
        {
            _ = await postRepo.GetSingleAsync(postId);
        }

        [HttpGet("{postId:int}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsOfPost(
            [FromRoute] int postId,
            [FromServices] ICommentRepository commentRepo
        )
        {
            await VerifyPostExists(postId);
            IEnumerable<CommentDto> comments = commentRepo.GetMany()
                .Where(c => c.PostId == postId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    AuthorUserId = c.UserId,
                    PostId = c.PostId
                })
                .AsEnumerable();
            return Ok(comments);
        }
        [HttpGet("{postId:int}/author")]
    public async Task<ActionResult<UserDto>> GetAuthorOfPost(
        [FromRoute] int postId,
        [FromServices] IUserRepository userRepo)
    {
        Post post = await postRepo.GetSingleAsync(postId);
        User author = await userRepo.GetSingleAsync(post.UserId);
        UserDto dto = new()
        {
            Id = author.Id,
            UserName = author.Username,
        };
        return Ok(dto);
    }
    }   
}
