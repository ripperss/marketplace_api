using marketplace_api.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace marketplace_api.Services.RedisService;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _redisCache;

    public RedisService(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task AddProductToCartAsync(string sessionToken, Role role, int productId, int quantity = 1)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        if (productId <= 0 || quantity <= 0)
        {
            throw new ArgumentException("ProductId and quantity must be greater than zero.");
        }

        var key = $"cart:{sessionToken}";

        var cart = await GetCartProductsAsync(sessionToken);

        var existingCartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
        if (existingCartProduct != null)
        {
            existingCartProduct.Quantity += quantity;
        }
        else
        {
            cart.CartProducts.Add(new CartProduct
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            });
        }

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

        await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(cart), options);
    }

    public async Task<Cart> GetCartProductsAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";

        var existingCart = await _redisCache.GetStringAsync(key);

        if (string.IsNullOrEmpty(existingCart))
        {
            return new Cart { CartProducts = new List<CartProduct>() };
        }

        var cart = JsonSerializer.Deserialize<Cart>(existingCart);

        return cart ?? new Cart { CartProducts = new List<CartProduct>() };
    }

    public async Task<CartProduct> GetProductFromCartAsync(string sessionToken, int productId)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        if (productId <= 0)
        {
            throw new ArgumentException("ProductId must be greater than zero.");
        }

        var cart = await GetCartProductsAsync(sessionToken);

        var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);

        if (cartProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found in cart.");
        }

        return cartProduct;
    }

    public async Task RemoveProductFromCartAsync(string sessionToken, int productId)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        if (productId <= 0)
        {
            throw new ArgumentException("ProductId must be greater than zero.");
        }

        var key = $"cart:{sessionToken}";

        var cart = await GetCartProductsAsync(sessionToken);

        var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
        if (cartProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found in cart.");
        }

        cart.CartProducts.Remove(cartProduct);

        await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(cart));
    }

    public async Task<List<CartProduct>> GetPaginatedCartProductsAsync(string sessionToken, int pageNumber, int pageSize)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        if (pageNumber <= 0 || pageSize <= 0)
        {
            throw new ArgumentException("Page number and page size must be greater than zero.");
        }

        var cart = await GetCartProductsAsync(sessionToken);

        var paginatedCartProducts = cart.CartProducts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return paginatedCartProducts;
    }

    public async Task DeleteAllCartAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";
        await _redisCache.RemoveAsync(key);
    }
}