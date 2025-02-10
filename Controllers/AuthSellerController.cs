using AutoMapper;
using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthSellerController : ControllerBase
{
    private readonly ILogger<AuthSellerController> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _validator;
    private readonly IAuthenticationService _authenticationService;
    private readonly Services.MailService _mail;

    public AuthSellerController(ILogger<AuthSellerController> logger
        , IMapper mapper
        , IValidator<UserDto> validator
        , IAuthenticationService authenticationService
        , Services.MailService mail
        )
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
        _authenticationService = authenticationService;
        _mail = mail;
    }

    [Route("log")]
    [HttpPost]
    public async Task<IActionResult> Login(UserDto userDto)
    {
        try
        {
            var valid = await _validator.ValidateAsync(userDto);
            if (!valid.IsValid)
            {
                return BadRequest("Данные не прошли вылидацию");
            }

            var user = _mapper.Map<User>(userDto)
                ?? throw new InvalidOperationException("Ошибка маппинга UserDto в User");
            user.Role = Role.Seller;

            var token = await _authenticationService.LoginAsync(user);
            HttpContext.Response.Cookies.Append("token", token);
            _logger.LogInformation("Успешная аутентификация продавца {UserId}", user.Id);

             //_mail.SendEmailAsync("вы успешно прогли аутификацию на подовца", userDto.Email);

            return Ok();
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Ошибка аутентификации: {Message}", ex.Message);
            return Problem(
                title: "Ошибка авторизации",
                detail: ex.Message,
                statusCode: (int)HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Критическая ошибка при аутентификации: {Message}", ex.Message);
            return Problem(
                title: "Внутренняя ошибка сервера",
                statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
