using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
 private readonly string filePath = "comments.json";

 public CommentFileRepository()
 {
    if(!File.Exists(filePath))
    {
        File.WriteAllText(filePath, "[]");
    }
 }

 public async Task<Comment> AddAsync(Comment comment)
 {
            string commentsAsJson = await File.ReadAllTextAsync(filePath);	        
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
	        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
	        comment.Id = maxId + 1;
	        comments.Add(comment);
	        commentsAsJson = JsonSerializer.Serialize(comments);
	        await File.WriteAllTextAsync(filePath, commentsAsJson);
	        return comment;
 }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);	        
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        Comment? commentToRemove = comments.SingleOrDefault(c=>c.Id == id);
        if(commentToRemove is null)
      {
         throw new InvalidOperationException($"Comment with ID {id} is not found");
      }
      comments.Remove(commentToRemove);
    }

    public IQueryable<Comment> GetMany()
 {
    string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
    List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
    return comments.AsQueryable();
 }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
    List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
    Comment? commentToGet = comments.SingleOrDefault(c=>c.Id == id);
    if(commentToGet is null)
      {
          throw new InvalidOperationException($"Comment with ID {id} is not found");
      }
      return commentToGet;
    }

    public Task UpdateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }
}
