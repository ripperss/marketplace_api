using marketplace_api.Models;

namespace marketplace_api.Services.RedisService;

public interface IRedisService
{
    Task<string> GetCashAsync(string key);
    Task CreateCashAsync(string key,string value, TimeSpan? expiration = null);
    Task DeleteAllCashAsync(string key);
    Task UpdateCashAsync(string key,string value);
    Task<bool> VerifyingExistenceOfKey(string key);
    Task<bool> DeleteValueCasheAsync<T>(string key,T value);
    Task AddProductToCartAsync(string sessionToken,Role role, string value);
    Task<List<string>> GetCartProductsAsync(string sessionToken);
}
