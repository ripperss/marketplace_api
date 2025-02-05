using marketplace_api.Models;
using marketplace_api.Repository.ProductRepository;
using marketplace_api.Services.UserService;
using marketplace_api.CustomExeption;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.Services.RedisService;
using System.Text.Json;
using marketplace_api.Repository.ProductViewHistoryRepository;

namespace marketplace_api.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserService _userService;
    private readonly IRedisService _redisService;
    private readonly IProductViewHistoryRepository _productViewHistoryRepository;

    public ProductService(
        IProductRepository productRepository
        ,IUserService userService
        , IRedisService redisService
        ,IProductViewHistoryRepository productViewHistoryRepository)
    {
        _productRepository = productRepository;
        _userService = userService;
        _redisService = redisService;
        _productViewHistoryRepository = productViewHistoryRepository;
    }

    public async Task<Product> CreateProductAsync(Product product, int sellerId)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var seller = await _userService.GetByIndexUserAsync(sellerId);
        product.SalesmanId = sellerId;
        product.Salesman.HashPassword = seller.HashPassword;
        product.Salesman.Email = seller.Email;
        product.Salesman.Name = seller.Name;
        product.SalesmanId = sellerId;

        var result = await _productRepository.CreateAsync(product);

        return result;
    }

    public async Task DeleteProductAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new NotFoundExeption(" Продукт с данным Id не найден");
        }
        await _productRepository.DeleteAsync(productId);

        var cahce = await _redisService.VerifyingExistenceOfKey("product");
        if (cahce)
        {
            var result = await _redisService.DeleteValueCasheAsync(productId.ToString(), product);
        }
    }

    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        var cahce = await _redisService.GetCashAsync("product");
        var products = JsonSerializer.Deserialize<List<Product>>(cahce);
        if(products == null)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        var product = products.FirstOrDefault(pr => pr.Id == productId);
        if(product == null)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        return product;
    }

    public async Task<User> GetSellerProductsAsync(int userId,int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if(product == null)
        {
            throw new NotFoundExeption("продукт не найден");
        }

        return product.Salesman;
    }

    public async Task<Product> PatchProductAsync(JsonPatchDocument<Product> productPatch, int productId)
    {
        if(productPatch == null)
        {
            throw new ArgumentNullException("продукт не может быть Null");
        }
        var product = await _productRepository.PatchAsync(productPatch,productId);

        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product, int productId)
    {
        if (product == null) 
        {
            throw new ArgumentNullException("");
        }
        await _productRepository.UpdateAsync(product, productId);

        var cahce = await _redisService.VerifyingExistenceOfKey("product");
        if (cahce)
        {
            var result = await _redisService.DeleteValueCasheAsync(productId.ToString(), product);
        }

        return product; 
    }
}
