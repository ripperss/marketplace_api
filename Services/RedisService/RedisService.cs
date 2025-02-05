using marketplace_api.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace marketplace_api.Services.RedisService;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _redisCash;

    public RedisService(IDistributedCache redisCash)
    {
        _redisCash = redisCash;
    }

    public async Task AddProductToCartAsync(string sessionToken, Role role, string productId)
    {
        if (string.IsNullOrEmpty(sessionToken) || string.IsNullOrEmpty(productId))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";

        var existingCart = await GetCashAsync(key);
        var productList = existingCart != null
            ? JsonSerializer.Deserialize<List<string>>(existingCart)
            : new List<string>();

        if (productList == null)
        {
            productList = new List<string>();
        }

        // Добавляем продукт в корзину
        if (!productList.Contains(productId))
        {
            productList.Add(productId);
        }

        TimeSpan expiration = role switch
        {
            Role.anonimus => TimeSpan.FromDays(30), 
            Role.Admin or Role.User => TimeSpan.FromDays(1), 
            _ => TimeSpan.FromDays(1) 
        };

        await CreateCashAsync(key, JsonSerializer.Serialize(productList), expiration);
    }

    /// <summary>
    /// Создаёт ключ в Redis с указанным значением и временем жизни.
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <param name="expiration">Время жизни ключа (по умолчанию 1 день)</param>
    /// <returns>Задача</returns>
    public async Task CreateCashAsync(string key, string value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromDays(1)
        };

        await _redisCash.SetStringAsync(key, value, options);
    }
        
    public async Task DeleteAllCashAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        await _redisCash.RemoveAsync(key);
    }

    public async Task<bool> DeleteValueCasheAsync<T>(string key, T value)
    {
        var cache = await GetCashAsync(key);
        if (cache == null)
        {
            return false;
        }

        var cacheList = JsonSerializer.Deserialize<List<T>>(cache);
        if (cacheList == null || !cacheList.Contains(value))
        {
            return false;
        }
        cacheList.Remove(value);

        var updateListCache = JsonSerializer.Serialize(cacheList);
        await UpdateCashAsync(key, updateListCache);

        return true;
    }

    public async Task<List<string>> GetCartProductsAsync(string sessionToken)
    {
        if (string.IsNullOrEmpty(sessionToken))
        {
            throw new ArgumentNullException(nameof(sessionToken));
        }

        var key = $"cart:{sessionToken}";

        var existingCart = await GetCashAsync(key);
        return existingCart != null
            ? JsonSerializer.Deserialize<List<string>>(existingCart)
            : new List<string>();
    }

    public async Task<string> GetCashAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }   

        try
        {
            return await _redisCash.GetStringAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis error: {ex.Message}");
            return null;
        }
    }

    public async Task UpdateCashAsync(string key, string value)
    {
        if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty (value))
        {
            throw new ArgumentNullException(nameof(key));
        }

        await _redisCash.SetStringAsync(key, value);
    }

    public async Task<bool> VerifyingExistenceOfKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        var value = await _redisCash.GetAsync(key);
        return value != null;
    }
}
