using AutoMapper;
using FluentValidation;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class AuthSellerController : ControllerBase
{
    private readonly ILogger<AuthSellerController> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator _validator;
    private readonly AuthenticationService _authenticationService;

    public AuthSellerController(ILogger<AuthSellerController> logger
        , IMapper mapper
        , IValidator validator
        , AuthenticationService authenticationService)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
        _authenticationService = authenticationService;
    }

    public IActionResult Login(UserDto userDto)
    {
        return Ok();
    }
}
