using AutoMapper;
using FluentValidation;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.CartManegementService;
using marketplace_api.Services.CartService;
using marketplace_api.Services.RedisService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;
    private readonly ICartManagementService _cartManagementService;
    private readonly JwtService _jwtService;
    private readonly IRedisService _redisService;

    public CartController(ICartService cartService
        , ILogger<CartController> logger
        , ICartManagementService cartManagementService
        , JwtService jwtService
        , IRedisService redisService)
    {
        _cartService = cartService;
        _logger = logger;
        _cartManagementService = cartManagementService;
        _jwtService = jwtService;
        _redisService = redisService;
    }


    [HttpPost]
    [Route("add_pdouct_cart/{productId:int}")]
    public async Task<IActionResult> AddProductAsync(int  productId)
    {
        Role role = Enum.Parse<Role>(HttpContext.Request.Cookies["role"] ?? "anonimus") ;
        var sessiontoken = HttpContext.Request.Cookies["sessionToken"];

        if (Request.Cookies.ContainsKey("token"))
        {
            var userId = _jwtService.GetIdUser(HttpContext);

            await _cartManagementService.AddProductOfCart(userId, sessiontoken, productId, Role.User);

            return StatusCode(201);
        }

        await _redisService.AddProductToCartAsync(sessiontoken, role, productId, 1);

        return StatusCode(201);
    }

    [HttpGet]
    [Route("f")]
    public async Task<IActionResult> GetProduct()
    {
        var sessiontoken = HttpContext.Request.Cookies["sessionToken"];

        var products = await _redisService.GetAllCartProductsAsync(sessiontoken);

        

        return Ok(products);
    }
}
