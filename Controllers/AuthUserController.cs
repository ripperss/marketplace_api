using AutoMapper;
using FluentValidation;
using Hangfire;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthUserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<UserDto> _validator; 
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly MailService _mailService;
    private readonly IValidator<UserLoginDto> _loginValid;

    public AuthUserController(
        IAuthenticationService authenticationService
        ,IMapper mapper
        ,IValidator<UserDto> validator
        , ICartService cartService
        , MailService mailService
        ,   IValidator<UserLoginDto> loginValid)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        _validator = validator;
        _cartService = cartService;
        _mailService = mailService;
        _loginValid = loginValid;
    }

    [HttpPost]
    [Route("reg")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            var validate =  await _validator.ValidateAsync(userDto);
            if (!validate.IsValid)
            {
                return BadRequest("Данные не прошли валидацию");
            }
            var user = _mapper.Map<User>(userDto);
            var result = await _authenticationService.RegisterUserAsync(user);

            BackgroundJob.Enqueue(() => _mailService.SendEmailAsync("Вы успешно зарегались под именем окунь поздравляю",userDto.Email ));

            return Ok(result);
        }

        catch (UserAlreadyExistsException ex)
        {
            var errorResponse = new
            {
                ErrorCode = "USER_ALREADY_EXISTS",
                Message = "User with the given username already exists. Please log in or use a different username."
            };
            return BadRequest(errorResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost]
    [Route("log")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        try
        {
            var validate = await _loginValid.ValidateAsync(userDto);
            if(!validate.IsValid)
            {
                return BadRequest("Данные не прошли валидацию");
            }

            var sessiontoken = HttpContext.Request.Cookies["sessionToken"];
            var user = _mapper.Map<User>(userDto);
            var token = await _authenticationService.LoginAsync(user, sessiontoken);


            HttpContext.Response.Cookies.Append("token", token);

            BackgroundJob.Enqueue(() => _mailService.SendEmailAsync("мистер окунь вы успешно залогинились",userDto.Email));

            return Ok();
        }

        catch (NotFoundExeption ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
