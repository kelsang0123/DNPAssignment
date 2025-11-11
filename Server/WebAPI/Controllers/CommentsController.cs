using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
           private readonly ICommentRepository commentRepo;

        public CommentsController(ICommentRepository commentRepo)
        {
            this.commentRepo = commentRepo;
        }
        [HttpPut("{id:int}")]
        public async Task<IResult> UpdateComment(
            [FromRoute] int id,
            [FromBody] UpdateCommentDto request)
        {
            Comment commentToUpdate = await commentRepo.GetSingleAsync(id);
            commentToUpdate.Body = request.Body;

            await commentRepo.UpdateAsync(commentToUpdate);
            return Results.NoContent();
        }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(
        [FromRoute] int id
    )
    {
        await commentRepo.DeleteAsync(id);
        return NoContent();
    }
    [HttpGet("{id:int}")]
        public async Task<ActionResult<CommentDto>> GetComment([FromRoute] int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);
        CommentDto dto = new()
        {
            Id = comment.Id,
            Body = comment.Body,
            AuthorUserId = comment.UserId,
            PostId = comment.PostId
        };
        return Ok(dto);
    }
        [HttpGet]
        public ActionResult<IEnumerable<CommentDto>> GetManyComments(
            [FromQuery]int?UserId=null,
            [FromQuery]int?PostId=null
        )
        {
           List<CommentDto> comments = commentRepo.GetMany()
           .Where(c => UserId == null || c.UserId == UserId)
           .Where(c => PostId == null || c.PostId == PostId)
           .Select(c=>new CommentDto
           {
            Id = c.Id,
            Body = c.Body,
            AuthorUserId = c.UserId,
            PostId = c.PostId               
           })
           .ToList();   
            return Ok(comments);
        }
    }
