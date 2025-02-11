using marketplace_api.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }
    
}
