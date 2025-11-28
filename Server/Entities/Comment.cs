using System;

namespace Entities;

public class Comment
{
    private Comment(){}
    public Comment(string Body, int AuthorUserId, int PostId)
    {
        this.Body = Body;
        this.AuthorUserId = AuthorUserId;
        this.PostId = PostId;
    }
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorUserId { get; set; }
    public int PostId { get; set; }
}
