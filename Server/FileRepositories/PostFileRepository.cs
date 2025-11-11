using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private readonly string filePath = "posts.json";

        public PostFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }
        public async Task<Post> AddAsync(Post post)
        {
            string postsAsJson = await File.ReadAllTextAsync(filePath);
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson) ?? [];
            int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 1;
            post.Id = maxId + 1;
            posts.Add(post);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            string postsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
            Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
            if (postToRemove is null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found");
            }
            posts.Remove(postToRemove);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
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
            Post? postToGet = posts.SingleOrDefault(p => p.Id == id);
            if (postToGet is null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found");
            }
            postsAsJson = JsonSerializer.Serialize(postToGet);
            await File.WriteAllTextAsync(filePath, postsAsJson);
            return postToGet;
        }

        public async Task UpdateAsync(Post post)
        {
            string postsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
            Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
            if (existingPost is null)
            {
                throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
            }
            posts.Remove(existingPost);
            posts.Add(post);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
    }
}