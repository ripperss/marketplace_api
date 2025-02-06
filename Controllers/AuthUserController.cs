using AutoMapper;
using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthUserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<UserDto> _validator; 
    private readonly IMapper _mapper;

    public AuthUserController(
        IAuthenticationService authenticationService
        ,IMapper mapper
        ,IValidator<UserDto> validator)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        _validator = validator;
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
    public async Task<IActionResult> Login(UserDto userDto)
    {
        try
        {
            var validate = await _validator.ValidateAsync(userDto);
            if(!validate.IsValid)
            {
                return BadRequest("Данные не прошли валидацию");
            }

            var user = _mapper.Map<User>(userDto);
            var token = await _authenticationService.LoginAsync(user);
            HttpContext.Response.Cookies.Append("token", token);
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
