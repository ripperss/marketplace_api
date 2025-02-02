using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.CustomExeption;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using marketplace_api.Services.AuthService;

namespace marketplace_api.Repository.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContext> _logger;
    private readonly JwtService _jwtService;

    public UserRepository(AppDbContext context,ILogger<AppDbContext> logger,JwtService jwtService)
    {
        _context = context;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var existingUser = _context.Users.FirstOrDefault(us => us.Name == user.Name);
        if (existingUser != null)
        {
            throw new NotFoundExeption("Данный пользователь уже существует");
        }

        _logger.LogInformation("Пользователь добавлен в бд @{user}",user);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;

    }

    public async Task DeleteUserAsync(int id)
    {
        var result = await _context.Users.FindAsync(id); 
        if (result == null)
        {
            throw new UserAlreadyExistsException("Данный пользователь уже существует");
        }

        _context.Users.Remove(result);
        await _context.SaveChangesAsync();

    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToList();
    }

    public async Task<User> GetByIndexUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new NotFoundExeption($"Пользователь с id {id} не найден");
        }

        return user;
    }

    public async Task<User> PatchUserAsync(JsonPatchDocument<User> user,int Id)
    {
        var userUpdate = await _context.Users.FindAsync(Id);

        if(userUpdate == null)
        {
            throw new NotFoundExeption("данный пользователь не существкет");
        }
        user.ApplyTo(userUpdate);
        await _context.SaveChangesAsync();

        return userUpdate;
    }

    public async Task<User> GetUserByNameAsync(string name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == name);

        if(user == null)
        {
            throw new NotFoundExeption("пользователя с данным именем не существует");
        }

        return user;
    }

    public async Task<User> UpdateUserAsync(User user, int Id)
    {
        var userUpdate = await _context.Users.FindAsync(Id);
        if (userUpdate == null)
        {
            throw new NotFoundExeption("Данный пользователь не сушествует");
        }

        userUpdate.Name = user.Name;
        userUpdate.Email = user.Email;
        userUpdate.HashPassword = user.HashPassword;
        await _context.SaveChangesAsync();

        return userUpdate;
    }
}
