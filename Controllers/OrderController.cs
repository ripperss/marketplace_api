using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class OrderController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IOrderService _orderService;
    private readonly JwtService _jwtService;

    public OrderController(ILogger logger
        , IOrderService orderService
        , JwtService jwtService)
    {
        _logger = logger;
        _orderService = orderService;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Route("orders")]
    [Authorize(Roles ="Admin,User,Seller")]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var userId = _jwtService.GetIdUser(HttpContext);
        var orders = await _orderService.GetOrdersAsync(userId);

        return Ok(orders);
    }

    [HttpGet]
    [Route("orders/{orderId:int}")]
    [Authorize(Roles = "Admin,User,Seller")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);

        return Ok(order);
    }

    [HttpPost]
    [Route("orders")]
    [Authorize(Roles ="User,Admin,Seller")]
    public async Task<IActionResult> CreateOrderAsync(CreateOrderRequest createOrderRequest)
    {
        await _orderService.CreateOrderAsync(createOrderRequest);

        return StatusCode(203);
    } 

    [HttpPut]
    [Route("orders/{orderId}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateStatusOrderAsync(int orderId, OrderStatus status)
    {
        await _orderService.OrderUpdateOfStatusAsync(status, orderId);

        return StatusCode(203);
    }
}
