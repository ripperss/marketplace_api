using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.CustomExeption;

namespace marketplace_api.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<User> RegisterUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("Данные при создание пользователя не могу быть раны null");
        }
        var result = _userRepository.RegisterAsync(user);

        if (result == null)
        {
            throw new UserAlreadyExistsException("Данный пользователь уже существует");
        }

        return result;

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
}
