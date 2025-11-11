namespace Entities;

public class User(string userName, string password)
{
    public int Id { get; set; }
    public string Username { get; set; } = userName;
    public string Password { get; set; } = password;
}
