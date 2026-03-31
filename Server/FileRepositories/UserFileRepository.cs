using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if(!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<User> AddAsync(User user)
    {
       string usersAsJson = await File.ReadAllTextAsync(filePath);
       List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
       int maxId = users.Count > 0 ? users.Max(u => u.Id) : 1;
       user.Id = maxId + 1;
       users.Add(user);
       usersAsJson = JsonSerializer.Serialize(users);
       await File.WriteAllTextAsync(filePath, usersAsJson);
       return user;
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
       List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
       User? userToRemove = users.SingleOrDefault(u=>u.Id == id);
       if(userToRemove is null)
        {
            throw new InvalidOperationException($"User with ID {id} not found");
        }
        users.Remove(userToRemove);
    }

    public IQueryable<User> GetMany()
    {
       string usersAsJson = File.ReadAllTextAsync(filePath).Result;
       List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
       return users.AsQueryable();
    }

    public async Task<User> GetSingleAsync(int id)
    {
    string usersAsJson = File.ReadAllTextAsync(filePath).Result;
    List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
    User? userToGet = users.SingleOrDefault(u=>u.Id == id);
       if(userToGet is null)
        {
            throw new InvalidOperationException($"User with ID {id} not found");
        }
    return userToGet;
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}
