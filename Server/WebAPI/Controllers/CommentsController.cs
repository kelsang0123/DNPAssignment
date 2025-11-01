using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using RepositoryContracts;
using FileRepositories;
using DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
           private readonly ICommentRepository commentRepo;

        public CommentsController(ICommentRepository commentRepo)
        {
            this.commentRepo = commentRepo;
        }
        [HttpPatch("{id:int}")]
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
        public async Task<IResult> DeleteComment(
            [FromRoute] int id
        )
        {
              await commentRepo.DeleteAsync(id);
            return Results.NoContent();
        }
        [HttpGet]
        public async Task<IResult> GetManyComments(
            [FromQuery]string?Body=null,
            [FromQuery]int?UserId=null,
            [FromQuery]int?PostId=null
        )
        {
           List<CommentDto> commentDtos = commentRepo.GetMany().Select(c=>new CommentDto
           {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId               
           }).ToList();   
            return Results.Ok(commentDtos);
        }
    }
}
