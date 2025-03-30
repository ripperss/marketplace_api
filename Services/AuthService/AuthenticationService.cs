using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.Identity;
using marketplace_api.Services.CartService;
using marketplace_api.Services.RedisService;
using Hangfire;
using marketplace_api.Services.UserService;
using marketplace_api.Services.CartManegementService;
using marketplace_api.ModelsDto;

namespace marketplace_api.Services.AuthService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;
    private ICartManagementService _cartManagementService;

    public AuthenticationService(IUserService userRepository
        , JwtService jwtService
        , ICartManagementService cartManagementService)
    {
        _userService = userRepository;
        _jwtService = jwtService;
        _cartManagementService = cartManagementService;
    }

    public async Task<string> RegisterUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var passwordHasher = new PasswordHasher<User>();
        user.HashPassword = passwordHasher.HashPassword(user, user.HashPassword);

        await _userService.CreateUserAsync(user);

        return "Пользователь создан";
    }

    public async Task<LoginResult> LoginAsync(User user, string sessiontoken)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var existingUser = await _userService.GetUserByEmailAsync(user.Email);

        if (existingUser == null)
        {
            throw new NotFoundExeption("User not found");
        }

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.HashPassword, user.HashPassword);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException("Invalid password");
            
        }

        BackgroundJob.Enqueue(() => _cartManagementService.CreateOrUpdateCart(existingUser.Id, sessiontoken));
        
        var token = _jwtService.GenerateJwtToken(existingUser);

        var loginResult = new LoginResult()
        {
            Token = token,
            Name = existingUser.Name,
            Role = existingUser.Role,
        };
        return loginResult;
    } 
}