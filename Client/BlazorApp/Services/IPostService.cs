using DTOs;
namespace BlazorApp.Services;

public interface IPostService
{
public Task<PostDto> AddPostAsync(CreatePostDto request);
public Task<IEnumerable<PostDto>> GetPosts();
public Task<PostDto> GetSinglePost(int postId);
    Task<UserDto> GetAuthorOfPost(int authorId);
 Task<List<CommentDto>> GetCommentsForPost(int postId);
  Task<CommentDto> AddComment(CreateCommentDto createCommentDto);
}
