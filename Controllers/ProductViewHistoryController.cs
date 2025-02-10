using AutoMapper;
using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.ProductViewHistoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class ProductViewHistoryController : ControllerBase
{
    private readonly IProductViewHistoryService _productViewHistoryService;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductDto> _validator;
    private readonly ILogger<ProductViewHistoryController> _logger;
    private readonly JwtService _jwtService;

    public ProductViewHistoryController(
        IProductViewHistoryService productViewHistoryService
        , IMapper mapper
        , IValidator<ProductDto> validator
        , ILogger<ProductViewHistoryController> logger
        , JwtService jwtService)
    {
        _productViewHistoryService = productViewHistoryService;
        _mapper = mapper;
        _jwtService = jwtService;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    [Route("history")]
    [Authorize(Roles ="Admin,User,Seller")]
    public async Task<IActionResult> GetHistory()
    {
        try
        {
            var userId = _jwtService.GetIdUser(HttpContext);
            _logger.LogInformation("Получение истории просмотров товаров для пользователя с ID {UserId}", userId);

            var history = await _productViewHistoryService.GetAllHistoryAsync(userId);

            var products = _mapper.Map<List<ProductDto>>(history);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении истории просмотров товаров.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Route("add_history/{productId}")]
    [Authorize(Roles = "Admin,User,Seller")]
    public async Task<IActionResult> AddProductHistoryAsync(ProductDto productDto,int productId)
    {
        try
        {
            var valid = _validator.Validate(productDto);
            if (!valid.IsValid)
            {
                return BadRequest(valid.Errors);
            }

            var product = _mapper.Map<Product>(productDto);
            product.Id = productId;

            var userId = _jwtService.GetIdUser(HttpContext);

            await _productViewHistoryService.AddHistoryAsync(product, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при добавлении товара в историю просмотров.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpDelete]
    [Route("del/{productId}")]
    [Authorize(Roles = "User,Admin,Seller")]
    public async Task<IActionResult> DeleteProductOfHistoryAsync(int productId)
    {
        try
        {
            int userId = _jwtService.GetIdUser(HttpContext);

            await _productViewHistoryService.DeleteHistoryAsync(userId, productId);

            return NotFound();
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Товар с ID {ProductId} не найден в истории просмотров пользователя с ID {UserId}", productId, _jwtService.GetIdUser(HttpContext));
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при удалении товара из истории просмотров.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpGet]
    [Route("pr_history/{productId}")]
    [Authorize(Roles = "User, Admin, Seller")]
    public async Task<IActionResult> GetProductOfHistory(int productId)
    {
        try
        {
            var userId = _jwtService.GetIdUser(HttpContext);

            _logger.LogInformation("Получение товара с ID {ProductId} из истории просмотров для пользователя с ID {UserId}", productId, userId);
            var product = await _productViewHistoryService.GetHistoryAsync(userId, productId);

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Товар с ID {ProductId} не найден в истории просмотров пользователя с ID {UserId}", productId, _jwtService.GetIdUser(HttpContext));
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении товара из истории просмотров.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }

    [HttpPut]
    [Route("history/{productId}")]
    public async Task<IActionResult> UpdateProductOfHistoryAsync(ProductDto productDto, int productId)
    {
        try
        {
            var valid = _validator.Validate(productDto);
            if (valid.IsValid)
            {
                return BadRequest(valid.Errors);
            }

            var product = _mapper.Map<Product>(productDto);

            var userId = _jwtService.GetIdUser(HttpContext);

            await _productViewHistoryService.UpdateHistoryAsync(product, userId, productId);

            return Ok(productDto);
        }
        catch (NotFoundExeption ex)
        {

            _logger.LogWarning(ex, "Товар с ID {ProductId} не найден в истории просмотров пользователя с ID {UserId} для обновления", productId, _jwtService.GetIdUser(HttpContext));
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обновлении товара в истории просмотров.");
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }
}
