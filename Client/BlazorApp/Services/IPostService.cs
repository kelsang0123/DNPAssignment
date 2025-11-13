
namespace BlazorApp.Services;

public interface IPostService
{
public async Task<PostDto> AddPostAsync(CreatePostDto request);
public async Task<PostDto> GetSinglePost(int id);
public IQueryable<PostDto> GetManyPosts();
public async Task UpdatePostAsync(int id, UpdatePostDto request);
public async Task DeletePostAsync(int id);
public async Task<CommentDto> AddComment(int postId, CreateCommentDto request, IUserService userService, ICommentService commentService);
public async Task<IEnumerable<CommentDto>> GetCommentsOfPostAsync(int id, ICommentService commentService);
}
