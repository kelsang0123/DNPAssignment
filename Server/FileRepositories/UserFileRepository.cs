using Entities;
using RepositoryContracts;
using System.Text.Json;
namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string filePath = "users.json";

        public UserFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }
        public async Task<User> AddAsync(User user)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;           //! in the end means users is not incomplete
            int maxId = users.Count>0? users.Max(u => u.Id) : 0; 
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
            User? userToRemove = users.SingleOrDefault(p => p.Id == id);
            if (userToRemove is null)
            {
                throw new InvalidOperationException($"User with ID '{id}' not found");
            }
            users.Remove(userToRemove);
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }

        public IQueryable<User> GetMany()
        {
            if(!File.Exists(filePath))
            {
                return Enumerable.Empty<User>().AsQueryable();
            }
            string usersAsJson = File.ReadAllText(filePath);

            if(string.IsNullOrWhiteSpace(usersAsJson))
            {
                return Enumerable.Empty<User>().AsQueryable();
            }
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson,
            new JsonSerializerOptions{PropertyNameCaseInsensitive = true})
            ?? new List<User>();

return users.AsQueryable();
        }

        public async Task<User> GetSingleAsync(int id)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!; // this means the file is never empty or null. //if usersAsJson is empty, this will throw a NullReferenceException
            User? userToGet = users.SingleOrDefault(u => u.Id == id);
            if (userToGet is null)
            {
                throw new InvalidOperationException($"User with ID '{id}' not found");
            }
            return userToGet;
        }

        public async Task UpdateAsync(User user)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson) ?? new List<User>();
            User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser is null)
            {
                throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
            }
            //one way
            users.Remove(existingUser);
            users.Add(user);
            //another way
           /* int index = users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            users[index] = user;
            */
            usersAsJson = System.Text.Json.JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }
}