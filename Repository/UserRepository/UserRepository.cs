using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.CustomExeption;

namespace marketplace_api.Repository.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContext> _logger;

    public UserRepository(AppDbContext context,ILogger<AppDbContext> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> RegisterAsync(User user)
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
        _context.Users.Remove(result);

    }

    public Task DeleteUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToList();
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
