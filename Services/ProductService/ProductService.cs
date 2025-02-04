using marketplace_api.Models;
using marketplace_api.Repository.ProductRepository;
using marketplace_api.Services.UserService;
using marketplace_api.CustomExeption;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.Services.RedisService;
using System.Text.Json;

namespace marketplace_api.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserService _userService;
    private readonly IRedisService _redisService;

    public ProductService(IProductRepository productRepository,IUserService userService, IRedisService redisService)
    {
        _productRepository = productRepository;
        _userService = userService;
        _redisService = redisService;
    }

    public async Task<Product> CreateProductAsync(Product product, int sellerId)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var seller = await _userService.GetByIndexUserAsync(sellerId);
        product.Salesman = seller;
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
        //TODO нужно прописать удаление из истории просмотров 
        //TODO нужно прописать удаление из истории покупок 
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

    public Task<Product> GetSellerProductsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> PatchProductAsync(JsonPatchDocument<User> product, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateProductAsync(Product product, int userId)
    {
        throw new NotImplementedException();
    }
}
