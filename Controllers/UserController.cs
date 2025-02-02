using AutoMapper;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class UserController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(
        IUserService userService
        ,JwtService jwtService
        ,IMapper mapper)
    {
        _jwtService = jwtService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUserAsync(UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest();
        }
        var user = _mapper.Map<User>(userDto);

        await _userService.CreateUserAsync(user);

        return Ok("Пользоватеь успекшно создан");
    }

    [HttpGet]
    [Route("Admin")]
    [Authorize(Roles ="User")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        

        return Ok(_mapper.Map<List<UserDto>>(users));
    }
}
