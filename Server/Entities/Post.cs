using System;

namespace Entities;

public class Post
{
    private Post(){}
    public Post(string title, string body, int authorUserId)
    {
        Title = title;
        Body = body;
        AuthorUserId = authorUserId;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int AuthorUserId { get; set; }
    public List<Comment> Comments{get; set;}
    public User User { get; set; }
}
