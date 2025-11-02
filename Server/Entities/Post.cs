using System;

namespace Entities;

public class Post
{
    public Post(string title, string body, int userId, string userName)
    {
        Title = title;
        Body = body;
        UserId = userId;
        UserName = userName;
    }

    public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public string UserName{ get; set;}
}
