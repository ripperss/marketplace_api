using marketplace_api.Models;
using marketplace_api.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthUserController : ControllerBase
{
   private readonly IUserService _userService;

    public AuthUserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost]
    [Route("reg")]
    public async Task<IActionResult> Register(User user)
    {
        try
        {
            var passwordHash = new PasswordHasher<User>().HashPassword(user, user.HashPassword);
            user.HashPassword = passwordHash;

            await _userService.RegisterUserAsync(user);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("log")]
    public IActionResult Loggin(string hashPassword,string email)
    {
        return BadRequest();
    }
}
