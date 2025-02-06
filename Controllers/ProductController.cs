using AutoMapper;
using FluentValidation;
using marketplace_api.CustomExeption;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.ProductService;
using marketplace_api.Services.ProductViewHistoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace marketplace_api.Controllers;

[ApiController]
[Route("{controller}")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductDto> _validator;
    private readonly ILogger<ProductController> _logger;
    private readonly JwtService _jwtService;
    private readonly IProductViewHistoryService _productViewHistoryService;

    public ProductController(IProductService productService
        , IMapper mapper
        , IValidator<ProductDto> validator
        , JwtService jwtService
        ,ILogger<ProductController> logger
        ,IProductViewHistoryService productViewHistoryService)
    {
        _productService = productService;
        _mapper = mapper;
        _validator = validator;
        _jwtService = jwtService;
        _logger = logger;
        _productViewHistoryService = productViewHistoryService;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles ="Seller,Admin")]
    public async Task<IActionResult> CreateProductAsync(ProductDto productDto)
    {
        _logger.LogInformation("Начато создание продукта");

        try
        {
            var valid = _validator.Validate(productDto);
            if (!valid.IsValid)
            {
                _logger.LogWarning("Ошибка валидации продукта: {Ошибки}", valid.Errors);
                return BadRequest("продукт не прошел валидацию");
            }

            var product = _mapper.Map<Product>(productDto);
            var userId = _jwtService.GetIdUser(HttpContext);
            var createProducts = await _productService.CreateProductAsync(product, userId);

            return Ok(createProducts);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при создании продукта");
            return StatusCode(500, "Произошла ошибка при создании продукта.");
        }
    }

    [HttpGet]
    [Route("product/{productId:int}")]
    public async Task<IActionResult> GetProductAsync(int productId)
    {
        _logger.LogInformation("Получение продукта с ID: {ProductId}", productId);

        try
        {
            var product = await _productService.GetProductAsync(productId);

            var userId = _jwtService.GetIdUser(HttpContext);
            
            await _productViewHistoryService.AddHistoryAsync(product,userId);

            return Ok(_mapper.Map<ProductDto>(product));
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении продукта с ID {ProductId}", productId);
            return StatusCode(500, "Произошла ошибка при получении продукта.");
        }
    }

    [HttpGet]
    [Route("top_product")]
    public async Task<IActionResult> GetTopProductAsync()
    {
        _logger.LogInformation("Получение списка топовых продуктов");

        try
        {
            var products = await _productService.GetAllProductsAsync();

            return Ok(_mapper.Map<List<ProductDto>>(products));
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении топовых продуктов");
            return StatusCode(500, "Произошла ошибка при получении топовых продуктов.");
        }
    }

    [HttpPut]
    [Route("update/{productId}")]
    [Authorize(Roles ="Admin,Seller")]
    public async Task<IActionResult> UpdateProductAsync(ProductDto productDto,int productId)
    {
        _logger.LogInformation("Обновление продукта с ID: {ProductId}", productId);

        try
        {
            var validator = _validator.Validate(productDto);
            if (!validator.IsValid)
            {
                return BadRequest("Данные не валидны");
            }

            var product = _mapper.Map<Product>(productDto);

            var newProduct = await _productService.UpdateProductAsync(product, productId);

            _logger.LogInformation("Продукт с ID {ProductId} успешно обновлен", productId);
            return Ok(newProduct);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обновлении продукта с ID {ProductId}", productId);
            return StatusCode(500, "Произошла ошибка при обновлении продукта.");
        }
    }

    [HttpPatch]
    [Route("patch{productId}")]
    [Authorize(Roles ="Admin,Seller")]
    public async Task<IActionResult> PatchProductAsync(JsonPatchDocument<Product> productPatch, int productId)
    {
        _logger.LogInformation("Частичное обновление продукта с ID: {ProductId}", productId);


        try
        {
            var product = await _productService.PatchProductAsync(productPatch, productId);

            _logger.LogInformation("Продукт с ID {ProductId} успешно обновлен частично", productId);
            return Ok(product);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при частичном обновлении продукта с ID {ProductId}", productId);
            return StatusCode(500, "Произошла ошибка при частичном обновлении продукта.");
        }
    }

    [HttpGet]
    [Route("name")]
    public async Task<IActionResult> GetByNameProductAsync(string name)
    {
        _logger.LogInformation("Поиск продуктов по имени: {Name}", name);

        try
        {
            var products = await _productService.GetProductByNameAsync(name);

            var result = _mapper.Map<ProductDto>(products);

            return Ok(result);
        }
        catch (NotFoundExeption ex)
        {
            _logger.LogError(ex, "Произошла ошибка при поиске продуктов по имени: {Name}", name);
            return StatusCode(500, "Произошла ошибка при поиске продуктов.");
        }
    }

    [HttpPost]
    [Route("saller{productId}")]
    public async Task<IActionResult> GetSallerAsync(int productId)
    {
        try
        {
            var saller = await _productService.GetSellerProductsAsync(productId);

            return Ok(_mapper.Map<UserDto>(saller));
        }
        catch (NotFoundExeption ex)
        {
            return StatusCode(500, "продукт не найден");
        }
    }

}
