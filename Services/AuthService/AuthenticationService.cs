using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.Repository.UserRepository;
using Microsoft.AspNetCore.Identity;
using marketplace_api.Services.CartService;
using marketplace_api.Services.RedisService;
using Hangfire;
using marketplace_api.Services.UserService;

namespace marketplace_api.Services.AuthService;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;
    private readonly ICartService _cartService;
    private readonly IRedisService _redisService;

    public AuthenticationService(IUserService userRepository
        , JwtService jwtService
        , ICartService cartService
        , IRedisService redisService)
    {
        _userService = userRepository;
        _jwtService = jwtService;
        _cartService = cartService;
        _redisService = redisService;
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

        return "Пользователб создан";
    }

    public async Task<string> LoginAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var existingUser = await _userService.GetUserByNameAsync(user.Name);

        if (existingUser == null)
        {
            throw new NotFoundExeption("User not found");
        }
        existingUser.Role = user.Role;

        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.HashPassword, user.HashPassword);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException("Invalid password");
            
        }

        CreateOrUpdateCart();
        BackgroundJob.Enqueue(() => _cartService.CreateCartAsync(existingUser.Id));
        
        
        var token = _jwtService.GenerateJwtToken(existingUser);
        return token;
    }

    private async Task CreateOrUpdateCart()
    {

    }
}
/*
 * 
 * var succsess = _cartService.CreateCartAsync(existingUser.Id)
 *    var cart = await _cartService.GetCart()
 *    
 *    var rediscahs = redisService.GetAll
 *    foreach( var f in rediscahs) {
 *          cart.AddProduct(f.ProductId, existingUser.Id)

 */