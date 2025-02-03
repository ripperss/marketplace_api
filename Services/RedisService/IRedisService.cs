namespace marketplace_api.Services.RedisService;

public interface IRedisService
{
    Task<string> GetCashAsync(string key);
    Task CreateCashAsync(string key,string value);
    Task DeleteCashAsync(string key);
    Task UpdateCashAsync(string key,string value);
    Task<bool> VerifyingExistenceOfKey(string key);


}
