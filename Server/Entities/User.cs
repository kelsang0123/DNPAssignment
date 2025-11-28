namespace Entities;

public class User
{
    private User(){} 
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Post> posts{get; set;}
    public List<Comment> comments{get; set;}
}
