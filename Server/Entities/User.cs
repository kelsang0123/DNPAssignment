namespace Entities;

public class User
{
    public User(string userName, string password)
    {
        Username = userName;
        Password = password;
    }

    private User(){} 
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Post> posts{get; set;}
    public List<Comment> comments{get; set;}
}
