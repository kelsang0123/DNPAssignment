using System.Net.Http;
using System.Text.Json;
using DTOs;
namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
     public async Task<PostDto> CreatePostAsync(CreatePostDto request)
    {
          HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if(!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
     public async Task<IEnumerable<PostOnlyDto>> GetPosts()
    {
        return await client.GetFromJsonAsync<IEnumerable<PostOnlyDto>>("api/posts")
                 ?? Enumerable.Empty<PostOnlyDto>();
    }
    public async Task<PostDto> GetSinglePost(int id, bool includeAuthor, bool includeComments)
    {
        return await client.GetFromJsonAsync<PostDto>("api/posts/{id, includeAuthor, includeComments}");
    }
    public async Task<CommentDto> AddComment(int id, CreateCommentDto request)
    {
     HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
     string response = await httpResponse.Content.ReadAsStringAsync();
     if(!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
         return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> AddComment(int postId, CreateCommentDto request, IUserService userService, ICommentService commentService)
    {
        throw new NotImplementedException();
    }
}
