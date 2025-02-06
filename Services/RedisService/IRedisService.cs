using marketplace_api.Models;

namespace marketplace_api.Services.RedisService;

public interface IRedisService
{
    Task AddProductToCartAsync(string sessionToken, Role role, string productId);
    Task<List<string>> GetCartProductsAsync(string sessionToken);
    Task DeleteAllCashAsync(string key);
}
