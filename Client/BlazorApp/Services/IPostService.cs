using DTOs;
namespace BlazorApp.Services;

public interface IPostService
{
public Task<PostDto> CreatePostAsync(CreatePostDto request);
public Task<IEnumerable<PostOnlyDto>> GetPosts();
public Task<PostDto> GetSinglePost(int id, bool includeAuthor, bool includeComments);
public Task UpdatePostAsync(int id, UpdatePostDto request);
public Task DeletePostAsync(int id);
public Task<CommentDto> AddComment(int postId, CreateCommentDto request, IUserService userService, ICommentService commentService);

}
