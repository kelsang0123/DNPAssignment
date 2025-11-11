using System;

namespace Entities;

public class Post
{
    public Post(string title, string body, int authorUserId)
    {
        Title = title;
        Body = body;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}
