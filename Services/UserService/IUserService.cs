using marketplace_api.Models;
using marketplace_api.ModelsDto;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Services.UserService;

public interface IUserService
{
    Task<User> GetByIndexUserAsync(int id);
    Task<User> UpdateUserAsync(User user,int Id);
    Task DeleteUserAsync(int userId);
    Task<User> PatchUserAsync(JsonPatchDocument<User> user,int Id);
    Task<User> GetUserByNameAsync(string name);
    Task<User> CreateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByEmailAsync(string email);

}
