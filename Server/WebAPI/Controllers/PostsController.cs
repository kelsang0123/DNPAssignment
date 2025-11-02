using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using RepositoryContracts;
using FileRepositories;
using DTOs;
using Microsoft.OpenApi;

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
            Post post = new(request.Title, request.Body, request.UserId, request.UserName);
            Post created = await postRepo.AddAsync(post);
            return Results.Created($"/posts/{created.Id}", created);
        }
        [HttpPatch("{id:int}")]
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
        [HttpGet("{id:int}")]
        public async Task<IResult> GetSinglePost(
             [FromRoute] int Id)
        {
            Post post = await postRepo.GetSingleAsync(Id);
            PostDto dto = new PostDto()
            {
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            return Results.Ok(dto);
        }
     [HttpGet]
     public async Task<IResult>GetPosts(
                [FromQuery]string?title=null,
                [FromQuery]string?body=null,
                [FromQuery]int?userId=null)
        {
            List<PostDto> posts = postRepo.GetManyAsync().Select( p=> new PostDto
            {
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId,
            }).ToList();
            return Results.Ok(posts);
        }
        [HttpPost("{postId:int}/comments")]
        public async Task<IResult> AddComment(
            [FromRoute] int postId,
            [FromBody] CreateCommentDto request,
            [FromServices] ICommentRepository commentRepo
        )
        {
            Comment comment = new(request.Body, request.UserId, postId);
            Comment created = await commentRepo.AddAsync(comment);
            CommentDto resultDto = new()
            {
              Id = comment.Id,
              Body = comment.Body,
              UserId = comment.UserId,
              PostId = comment.PostId  
            };
            return Results.Created($"/Comments/{created.Id}",resultDto);
        }

        [HttpGet("{postId:int}/comments")]
        public async Task<IResult> GetCommentsOfPost(
            [FromRoute] int id,
            [FromServices] ICommentRepository commentRepo
        )
        {
            List<CommentDto> comments = commentRepo.GetMany().Select(c => new CommentDto
            {
                Id = c.Id,
                Body = c.Body,
                UserId = c.UserId,
                PostId = id
            }).ToList();
            return (IResult)Ok(comments);
        }
    }
}
