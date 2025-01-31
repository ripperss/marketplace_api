using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.CustomExeption;
using Microsoft.AspNetCore.Identity;
using marketplace_api.Services.AuthService;

namespace marketplace_api.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    
    public UserService(IUserRepository userRepository,JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIndexUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<JsonPatchDocument<User>> PatchUserAsync(JsonPatchDocument<User> user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByNameAsync(string name)
    {
        if(name == null)
        {
            throw new ArgumentNullException("name");
        }

        var user = await _userRepository.GetUserByNameAsync(name);

        return user;
    }

    public Task<User> CreateUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }

        var result = _userRepository.GetUserByNameAsync(user.Name);
        if (result != null)
        {
            throw new UserAlreadyExistsException(user.Name);
        }

        _userRepository.CreateUserAsync(user);
        return result;
    }
}
