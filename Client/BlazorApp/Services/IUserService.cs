using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
public Task<UserDto> AddUserAsync(CreateUserDto request);
public Task<UserDto> GetSingleUser(int id);
public IQueryable<UserDto> GetManyUsers();
public Task UpdateUserAsync(int id, UpdateUserDto request);
public Task DeleteUserAsync(int id);
}
