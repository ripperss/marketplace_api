using AutoMapper;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthUserController : ControllerBase
{
   private readonly IUserService _userService;
   private  readonly IMapper _mapper;
   private readonly ILogger<AuthUserController> _logger;    

    public AuthUserController(IUserService userService,IMapper mapper, ILogger<AuthUserController> logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost]
    [Route("reg")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            var passwordHash = new PasswordHasher<UserDto>().HashPassword(userDto, userDto.HashPassword);
            userDto.HashPassword = passwordHash;

            var user = _mapper.Map<User>(userDto);

            user.Role = Role.User;

            await _userService.RegisterUserAsync(user);

            return Ok(userDto);
        }
        catch (NotFoundExeption ex)
        {
            return BadRequest(ex.Message + "Данный пользователь не найден");
        }
        catch (UserAlreadyExistsException ex)
        {
            var errorResponse = new
            {
                ErrorCode = "USER_ALREADY_EXISTS",
                Message = "Пользователь с такими данными уже зарегистрирован. Пожалуйста, войдите или используйте другой логин."
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    [Route("log")]
    public IActionResult Loggin(string hashPassword,string email)
    {
        return BadRequest();
    }
}
