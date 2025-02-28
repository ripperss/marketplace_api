using Hangfire;
using Hangfire.PostgreSql.Properties;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.OrderService;
using marketplace_api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;
    private readonly JwtService _jwtService;
    private readonly MailService _mailService;
    private readonly IUserService _userService;

    public OrderController(ILogger<OrderController> logger
        , IOrderService orderService
        , JwtService jwtService
        , MailService mailService,
        IUserService userService)
    {
        _logger = logger;
        _orderService = orderService;
        _jwtService = jwtService;
        _mailService = mailService;
        _userService = userService;
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
        var userId = _jwtService.GetIdUser(HttpContext);
        createOrderRequest.UserId = userId;

        await _orderService.CreateOrderAsync(createOrderRequest);

        var user = await _userService.GetByIndexUserAsync(userId);
        BackgroundJob.Enqueue(() => _mailService.SendEmailAsync("Ваш заказ оформлен",user.Email));

        return StatusCode(201);
    } 

    [HttpPut]
    [Route("orders/{orderId}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateStatusOrderAsync(int orderId, OrderStatus status)
    {
        await _orderService.OrderUpdateOfStatusAsync(status, orderId);

        return StatusCode(201);
    }

    [HttpPut]
    [Route("order_update{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderAsync(OrderDto orderDto, int orderId)
    {
        var userId = _jwtService.GetIdUser(HttpContext);
        orderDto.UserId = userId;

        await _orderService.UpdateOrderAsync(orderDto, orderId);

        return StatusCode(201);
    }

    [HttpDelete]
    [Route("orders/{orderId}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> PickUpAnOrder(int orderId)
    {
        await _orderService.RemoveOrderFromDeliveryAsync(orderId);

        return NoContent();
    }

    [HttpGet]
    [Route("Allorders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders(DateTime date)
    {
        var orders = await _orderService.GetOrdersByDate(date);

        return Ok(orders);
    }
}
