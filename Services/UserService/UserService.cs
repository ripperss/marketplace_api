using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.CustomExeption;
using Microsoft.AspNetCore.Identity;
using marketplace_api.Services.AuthService;
using System.Net.WebSockets;
using AutoMapper;
using marketplace_api.ModelsDto;

namespace marketplace_api.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task DeleteUserAsync(int Id)
    {
        await _userRepository.DeleteUserAsync(Id);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users;
    }

    public Task<User> GetByIndexUserAsync(int id)
    {
        var user = _userRepository.GetByIndexUserAsync(id);
        if(user == null)
        {
            throw new NotFoundExeption("Пользователь не найден");
        }

        return user; 

    }

    public async Task<User> PatchUserAsync(JsonPatchDocument<User> user,int Id)
    {
        if( user == null)
        {
            throw new NullReferenceException();
        }
        var result = await _userRepository.PatchUserAsync(user, Id);

        return result;
    }

    public async Task<User> GetUserByNameAsync(string name)
    {
        if (name == null)
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

        _userRepository.CreateUserAsync(user);
        return result;
    }

    public async Task<User> UpdateUserAsync(User user, int Id)
    {
        if( user == null)
        {
            throw new ArgumentNullException("user");
        }
        var result = await _userRepository.UpdateUserAsync(user, Id);

        return result;
    }
}
