using marketplace_api.Models;
using marketplace_api.Repository.ProductRepository;
using marketplace_api.Services.ProductService;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace marketplace_api.Services.RedisService;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _redisCache;
    private readonly IProductService _productService;

    public RedisService(IDistributedCache redisCache, IProductService productService)
    {
        _redisCache = redisCache;
        _productService = productService;
    }

    private async Task<List<CartProduct>> GetCartProductsListAsync(string sessionToken)
    {
        var key = $"cart:{sessionToken}";
        var existingData = await _redisCache.GetStringAsync(key);

        return string.IsNullOrEmpty(existingData) ? new List<CartProduct>()
            : JsonSerializer.Deserialize<List<CartProduct>>(existingData)
            ?? new List<CartProduct>();
    }

    private async Task SaveCartProductsListAsync(string sessionToken, Role role, List<CartProduct> cartProducts)
    {
        var key = $"cart:{sessionToken}";

        TimeSpan expiration = role switch
        {
            Role.anonimus => TimeSpan.FromDays(30),
            Role.Admin or Role.User => TimeSpan.FromDays(1),
            _ => TimeSpan.FromDays(1)
        };

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(cartProducts), options);
    }

    public async Task AddProductToCartAsync(string sessionToken, Role role, int productId, int quantity = 1)
    {
        if (string.IsNullOrEmpty(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));
        if (productId <= 0 || quantity <= 0) throw new ArgumentException("ProductId и Quantity должны быть больше нуля.");

        var cartProducts = await GetCartProductsListAsync(sessionToken);
        var existingProduct = cartProducts.FirstOrDefault(cp => cp.ProductId == productId);

        if (existingProduct != null)
        {
            existingProduct.Quantity += quantity;
        }
        else
        {
            cartProducts.Add(new CartProduct { ProductId = productId, Quantity = quantity });
        }

        await SaveCartProductsListAsync(sessionToken, role, cartProducts);
    }

    public async Task<CartProduct> GetProductFromCartAsync(string sessionToken, int productId)
    {
        if (string.IsNullOrEmpty(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

        var cartProducts = await GetCartProductsListAsync(sessionToken);
        var product = cartProducts.FirstOrDefault(cp => cp.ProductId == productId);

        if (product == null) throw new KeyNotFoundException($"Товар с ID {productId} не найден в корзине.");

        return product;
    }

    public async Task RemoveProductFromCartAsync(string sessionToken, int productId)
    {
        if (string.IsNullOrEmpty(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

        var cartProducts = await GetCartProductsListAsync(sessionToken);
        var product = cartProducts.FirstOrDefault(cp => cp.ProductId == productId);

        if (product == null) throw new KeyNotFoundException($"Товар с ID {productId} не найден в корзине.");

        cartProducts.Remove(product);
        await SaveCartProductsListAsync(sessionToken, Role.User, cartProducts);
    }

    public async Task<List<CartProduct>> GetPaginatedCartProductsAsync(string sessionToken, int pageNumber, int pageSize)
    {
        if (string.IsNullOrEmpty(sessionToken)) 
            throw new ArgumentNullException(nameof(sessionToken));

        if (pageNumber <= 0 || pageSize <= 0) 
            throw new ArgumentException("PageNumber и PageSize должны быть больше нуля.");

        var cartProducts = await GetCartProductsListAsync(sessionToken);

        var cartProductsWithDetails = await LoadProductDetails(cartProducts);

        return cartProductsWithDetails
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    }

    public async Task DeleteAllCartAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

        var key = $"cart:{sessionToken}";
        await _redisCache.RemoveAsync(key);
    }

    public async Task<List<CartProduct>> GetAllCartProductsAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

        var cartProducts = await GetCartProductsListAsync(sessionToken);

        var cartProductsWithDetails = await LoadProductDetails(cartProducts);

        return cartProductsWithDetails;
    }

    private async Task<List<CartProduct>> LoadProductDetails(List<CartProduct> cartProducts)
    {
        var cartProductsWithDetails = new List<CartProduct>();
        foreach (var cartProduct in cartProducts)
        {
            try
            {
                var product = await _productService.GetProductAsync(cartProduct.ProductId);
                cartProduct.Product = product; 
                cartProductsWithDetails.Add(cartProduct);
            }
            catch (Exception ex)
            {
                throw new DllNotFoundException("продукт не найден");
            }
        }
        return cartProductsWithDetails;
    }
}