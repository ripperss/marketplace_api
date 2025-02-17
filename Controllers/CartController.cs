using AutoMapper;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.CartManegementService;
using marketplace_api.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly ICartManagementService _cartManagementService;
    private readonly IMapper _mapper; 

    public CartController(ICartService cartService
        , ILogger<CartController> logger
        , ICartManagementService cartManagementService
        , IMapper mapper)
    {
        _logger = logger;
        _cartManagementService = cartManagementService;
        _mapper = mapper;
    }


    [HttpPost]
    [Route("add_pdouct_cart/{productId:int}")]
    public async Task<IActionResult> AddProductAsync(int  productId)
    {
        try
        {
            await _cartManagementService.AddProductOfCart(productId, HttpContext);
            _logger.LogInformation("Продукт с ID {ProductId} успешно добавлен в корзину.", productId);

            return StatusCode(201);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Продукт с ID {ProductId} не найден.", productId);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при добавлении продукта с ID {ProductId} в корзину.", productId);
            return StatusCode(500, "Внутренняя ошибка сервера.");
        }
    }

    [HttpGet]
    [Route("products")]
    public async Task<IActionResult> GetProductsAsync()
    {
        try
        {
            var product = await _cartManagementService.GetAllProductOfCartAsync(HttpContext);

            return Ok(_mapper.Map<List<CartProductDto>>(product));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Произошла ошибка при получении списка продуктов в корзине.");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("cart_product/{productId}")]
    public async Task<IActionResult> GetPdouctAsync(int productId)
    {
        try
        {
            var cartProduct = await _cartManagementService.GetProductCartAsync(HttpContext, productId);
            _logger.LogInformation("Получен продукт с ID {ProductId} из корзины.", productId);

            return Ok(_mapper.Map<CartProductDto>(cartProduct));
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Продукт с ID {ProductId} не найден в корзине.", productId);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении продукта с ID {ProductId} из корзины.", productId);
            return StatusCode(500, "Внутренняя ошибка сервера.");
        }
    }

    [HttpGet]
    [Route("product_page/{page:int}")]
    public async Task<IActionResult> GetProductOfPage(int page)
    {
        try
        {
            var cartProductPage = await _cartManagementService.GetPageProdcutOfCart(HttpContext, page);
            _logger.LogInformation("Получена страница {Page} продуктов из корзины.", page);

            return Ok(_mapper.Map<List<CartProductDto>>(cartProductPage));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Некорректный номер страницы: {Page}.", page);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении страницы {Page} продуктов из корзины.", page);
            return StatusCode(500, "Внутренняя ошибка сервера.");
        }
    }

    [HttpDelete]
    [Route("product_del/{productId}")]
    public async Task<IActionResult> DeleProductAsync(int productId)
    {
        try
        {
            await _cartManagementService.DeleteProductCartAsync(HttpContext, productId);
            _logger.LogInformation("Продукт с ID {ProductId} удален из корзины.", productId);

            return NoContent();
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Продукт с ID {ProductId} не найден в корзине для удаления.", productId);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при удалении продукта с ID {ProductId} из корзины.", productId);
            return StatusCode(500, "Внутренняя ошибка сервера.");
        }
    }

    [HttpPut]
    [Route("update_cart")]
    public async Task<IActionResult> UpdateCartAsync(Cart newCart)
    {
        try
        {
            await _cartManagementService.UpdateCartAsync(HttpContext, newCart);
            _logger.LogInformation("Корзина обновлена.");

            return NoContent();
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogWarning(ex, "Корзина не найдена для обновления.");
            return NotFound(ex.Message); 
        }
        catch (ArgumentException ex) 
        {
            _logger.LogWarning(ex, "Некорректные данные корзины для обновления.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обновлении корзины.");
            return StatusCode(500, "Внутренняя ошибка сервера.");
        }
    }
}
