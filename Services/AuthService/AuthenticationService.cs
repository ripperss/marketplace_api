using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.Identity;
using marketplace_api.CustomExeption;

namespace marketplace_api.Services.AuthService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthenticationService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> RegisterUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var passwordHasher = new PasswordHasher<User>();
        user.HashPassword = passwordHasher.HashPassword(user, user.HashPassword);


        await _userRepository.CreateUserAsync(user);

        
        return "Пользователб создан";
    }

    public async Task<string> LoginAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var existingUser = await _userRepository.GetUserByNameAsync(user.Name);
        if (existingUser == null)
        {
            throw new NotFoundExeption("User not found");
        }

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.HashPassword, user.HashPassword);

        if (verificationResult == PasswordVerificationResult.Success)
        {
            var token = _jwtService.GenerateJwtToken(user);
            return token;
        }
        else
        {
            throw new UnauthorizedAccessException("Invalid password");
        }
    }
}