



namespace DTOs;

public class PostDto
{
    public int Id{get; set;}
    public string Title { get; set; }
    public string Body { get; set; }
    public int AuthorUserId { get; set; }
    public object Author { get; set; }
    public List<CommentDto> Comments { get; set; }
}