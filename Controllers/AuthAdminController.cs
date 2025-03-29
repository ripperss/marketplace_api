using AutoMapper;
using marketplace_api.CustomFilter;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("api/admin/auth")]
[ServiceFilter(typeof(CustomAdminValidteFilter))]
public class AuthAdminController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    
    public AuthAdminController(IMapper mapper, IAuthenticationService authenticationService)
    {
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(AdminDto adminDto)
    {
        if (adminDto.Code == "3347MM__rer")
        {
            var user = _mapper.Map<User>(adminDto);
            user.Role = Role.Admin;
            var sessiontoken = HttpContext.Request.Cookies["sessionToken"];
            var loginResult =  await _authenticationService.LoginAsync(user, sessiontoken);
            loginResult.AuthResult = 200;

            HttpContext.Response.Cookies.Append("token", loginResult.Token);
            return Ok(loginResult);
        }

        return BadRequest();
    }
}