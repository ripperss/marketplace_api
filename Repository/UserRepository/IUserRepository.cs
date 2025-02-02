using Azure;
using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Repository.UserRepository;

public interface IUserRepository
{
    Task<User> GetByIndexUserAsync(int id);
    Task<User> GetUserByNameAsync(string name);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user,int Id);
    Task DeleteUserAsync(int id);
    Task<User> PatchUserAsync(JsonPatchDocument<User> user,int Id);
}
