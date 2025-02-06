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

    public async Task AddProductToCartAsync(string sessionToken, Role role, string productId)
    {
        if (string.IsNullOrEmpty(sessionToken) || string.IsNullOrEmpty(productId))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";

        var existingCart = await GetCartProductsAsync(sessionToken);
        if (!existingCart.Contains(productId))
        {
            existingCart.Add(productId);
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

        await _redisCache.SetStringAsync(key, JsonSerializer.Serialize(existingCart), options);
    }

    public async Task<List<string>> GetCartProductsAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";

        var existingCart = await _redisCache.GetStringAsync(key);
        return existingCart != null
            ? JsonSerializer.Deserialize<List<string>>(existingCart)
            : new List<string>();
    }

    public async Task DeleteAllCashAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        await _redisCache.RemoveAsync(key);
    }
}




