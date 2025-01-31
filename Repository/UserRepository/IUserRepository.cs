using Azure;
using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Repository.UserRepository;

public interface IUserRepository
{
    Task<User> GetUserAsync();
    Task<User> GetByIndexUserAsync(int id);
    Task<User> GetUserByNameAsync(string name);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task DeleteUserAsync();
    Task<JsonPatchDocument<User>> PatchUserAsync(JsonPatchDocument<User> user);
    
}
