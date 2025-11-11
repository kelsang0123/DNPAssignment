
namespace DTOs;

public class PostDto
{
    public int Id;
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public UserDto Author { get; set; }
    public List<CommentDto> Comments { get; set; }
}