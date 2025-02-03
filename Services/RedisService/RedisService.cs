using Microsoft.Extensions.Caching.Distributed;

namespace marketplace_api.Services.RedisService;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _redisCash;

    public RedisService(IDistributedCache redisCash)
    {
        _redisCash = redisCash;
    }

    public async Task CreateCashAsync(string key, string value)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) 
        };

        await _redisCash.SetStringAsync(key, value, options);
    }
        
    public Task DeleteCashAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetCashAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCashAsync(string key, string value)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyingExistenceOfKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        var value = await _redisCash.GetAsync(key);
        return value != null;
    }
}
