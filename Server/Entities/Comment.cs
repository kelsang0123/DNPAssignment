using System;

namespace Entities;

public class Comment
{
    public Comment(string Body, int AuthorUserId, int PostId)
    {
        this.Body = Body;
        this.UserId = AuthorUserId;
        this.PostId = PostId;
    }
    public int Id { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}
