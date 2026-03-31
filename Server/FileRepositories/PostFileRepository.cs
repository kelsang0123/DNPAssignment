using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if(!File.Exists(filePath))
        {
            File.WriteAllTextAsync(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if(postToRemove is null)
        {
            throw new InvalidOperationException(
            $"Post with {id} not found.");
        }
        posts.Remove(postToRemove);
    } 
    
    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }


    public async Task<Post> GetSingleAsync(int id)
    {
     
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;   
        Post? postToGet = posts.FirstOrDefault(p=>p.Id == id);
        if(postToGet is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
       return postToGet;
    }

    public Task UpdateAsync(Post post)
    {
        throw new NotImplementedException();
    }
}
