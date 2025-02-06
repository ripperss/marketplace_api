using marketplace_api.Models;
using marketplace_api.Repository.ProductRepository;
using marketplace_api.Services.UserService;
using marketplace_api.CustomExeption;
using Microsoft.AspNetCore.JsonPatch;
using marketplace_api.Services.RedisService;
using System.Text.Json;
using marketplace_api.Repository.ProductViewHistoryRepository;
using Microsoft.IdentityModel.Tokens;

namespace marketplace_api.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserService _userService;
    private readonly IProductViewHistoryRepository _productViewHistoryRepository;

    public ProductService(
        IProductRepository productRepository,
        IUserService userService,
        IProductViewHistoryRepository productViewHistoryRepository)
    {
        _productRepository = productRepository;
        _userService = userService;
        _productViewHistoryRepository = productViewHistoryRepository;
    }

    public async Task<Product> CreateProductAsync(Product product, int sellerId)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var seller = await _userService.GetByIndexUserAsync(sellerId);

        product.UserId = sellerId;

        var result = await _productRepository.CreateAsync(product);

        return result;
    }

    public async Task DeleteProductAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new NotFoundExeption("Продукт с данным Id не найден");
        }
        await _productRepository.DeleteAsync(productId);
    }

    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        int limit = 100;
        var products =  await _productRepository.GetAllAsync();
        var topProducts = products
            .OrderByDescending(pr => pr.CountViewProduct)
            .Take(limit)
            .ToList();
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new NotFoundExeption("Продукт не найден");
        }

        return product;
    }

    public async Task<ICollection<Product>> GetProductByNameAsync(string name)
    {
        if(name == null)
        {
            throw new ArgumentNullException("name");
        }

        var products = await  _productRepository.GetByName(name);
        if(products.IsNullOrEmpty())
        {
            throw new NotFoundExeption("продукт с данным именем не найден");
        }
        
        return products;
    }

    public async Task<User> GetSellerProductsAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        var user = await _userService.GetByIndexUserAsync(product.UserId);
        if (product == null)
        {
            throw new NotFoundExeption("Продукт не найден");
        }

        return user;
    }
    

    public async Task<Product> PatchProductAsync(JsonPatchDocument<Product> productPatch, int productId)
    {
        if (productPatch == null)
        {
            throw new ArgumentNullException("Продукт не может быть Null");
        }
        var product = await _productRepository.PatchAsync(productPatch, productId);

        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product, int productId)
    {
        if (product == null)
        {
            throw new ArgumentNullException("");
        }
        await _productRepository.UpdateAsync(product, productId);

        return product;
    }
}
