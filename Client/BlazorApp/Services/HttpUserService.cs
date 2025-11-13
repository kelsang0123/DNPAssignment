using System.Text.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
    HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
    string response = await httpResponse.Content.ReadAsStringAsync();
    if (!httpResponse.IsSuccessStatusCode)
    {
        throw new Exception(response);
    }
    return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    })!;
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<UserDto> GetManyUsers()
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetSingleUser(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        throw new NotImplementedException();
    }
}
