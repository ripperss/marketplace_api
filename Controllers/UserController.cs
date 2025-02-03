using AutoMapper;
using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class UserController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IUserService _userService;
    private readonly IValidator<UserDto> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService
        ,JwtService jwtService
        ,IMapper mapper
        ,ILogger<UserController> logger
        ,IValidator<UserDto> validator)
    {
        _jwtService = jwtService;
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUserAsync(UserDto userDto)
    {
        _logger.LogInformation("Попытка создать нового пользователя.");

        var validationResult = await _validator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Создание пользователя не удалось из-за ошибок валидации: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var user = _mapper.Map<User>(userDto);
            await _userService.CreateUserAsync(user);
            _logger.LogInformation("Пользователь успешно создан.");
            return Ok("Пользователь успешно создан");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании пользователя.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpGet]
    [Route("users")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        _logger.LogInformation("Получение всех пользователей.");

        try
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            _logger.LogInformation("Успешно получены все пользователи.");
            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении всех пользователей.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpGet]
    [Route("user")]
    [Authorize(Roles ="User,Admin")]
    public async Task<IActionResult> GetUserAsync()
    {
        try
        {
            int userId = _jwtService.GetIdUser(HttpContext);
            _logger.LogInformation("Получение пользователя с ID: {UserId}", userId);
            User user = await _userService.GetByIndexUserAsync(userId);

            var userDto = _mapper.Map<UserDto>(user);
            _logger.LogInformation("Успешно получен пользователь с ID: {UserId}", userId);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении пользователя.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpGet]
    [Route("user/{id:int}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Получение пользователя по ID: {UserId}", id);
        try
        {
            var user = await _userService.GetByIndexUserAsync(id);

            var userDto = _mapper.Map<UserDto>(user);
            _logger.LogInformation("Успешно получен пользователь с ID: {UserId}", id);
            return Ok(userDto);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Пользователь не найден.");
            return NotFound(ex.Message); // 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении пользователя по ID: {UserId}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpDelete]
    [Route("del/{id:int}")]
    [Authorize(Roles ="Admin")] 
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        _logger.LogInformation("Попытка удалить пользователя с ID: {UserId}", id);
        try
        {
            await _userService.DeleteUserAsync(id);
            _logger.LogInformation("Пользователь с ID {UserId} успешно удален.", id);
            return NoContent();
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Пользователь не найден.");
            return NotFound(ex.Message); // 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении пользователя с ID: {UserId}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpPatch]
    [Route("putch")]
    [Authorize(Roles ="User,Admin")]
    public async Task<IActionResult> PatchUserAsync(JsonPatchDocument<UserDto> patch)
    {
        if (patch == null)
        {
            return BadRequest("Документ патча отсутствует.");
        }

        var userId = _jwtService.GetIdUser(HttpContext);
        _logger.LogInformation("Попытка частично обновить пользователя с ID: {UserId}", userId);

        try
        {
            var existingUser = await _userService.GetByIndexUserAsync(userId);

            var userDto = _mapper.Map<UserDto>(existingUser);

            patch.ApplyTo(userDto);

            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Частичное обновление пользователя не удалось из-за ошибок валидации: {Errors}", validationResult.Errors);
                return BadRequest(ModelState); 
            }

            _mapper.Map(userDto, existingUser);

            await _userService.UpdateUserAsync(existingUser,userId);

            _logger.LogInformation("Пользователь с ID {UserId} частично обновлен успешно.", userId);
            return Ok("Пользователь частично обновлен успешно.");
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Пользователь не найден.");
            return NotFound(ex.Message); // 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при частичном обновлении пользователя с ID: {UserId}", userId);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }

    }

    [HttpPut]
    [Route("us_update")]
    [Authorize(Roles="User,Admin")]
    public async Task<IActionResult> UpdateUserAsync(UserDto userDto)
    {
        var valid = await _validator.ValidateAsync(userDto);

        if (!valid.IsValid)
        {
            _logger.LogWarning("обнавление пользователя не удалось из-за ошибок валидации: {Errors}", valid.Errors);
            return BadRequest(valid.Errors);
        }
        try
        {
            var user = _mapper.Map<User>(userDto);
            var userId = _jwtService.GetIdUser(HttpContext);
            var result = await _userService.UpdateUserAsync(user, userId);

            return Ok(userDto);
        }
        catch(NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Пользователь не найден.");
            return NotFound(ex.Message); // 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении пользователя");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }


    }
}
