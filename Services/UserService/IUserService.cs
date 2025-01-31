using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Services.UserService;

public interface IUserService
{
    Task<User> GetUserAsync();
    Task<User> GetByIndexUserAsync(int id);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task DeleteUserAsync();
    Task<JsonPatchDocument<User>> PatchUserAsync(JsonPatchDocument<User> user);
    Task<User> GetUserByNameAsync(string name);
    Task<User> CreateUserAsync(User user);
    
}
