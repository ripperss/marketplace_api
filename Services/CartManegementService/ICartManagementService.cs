using marketplace_api.Models;
using Org.BouncyCastle.Bcpg;

namespace marketplace_api.Services.CartManegementService;

public interface ICartManagementService
{
    Task CreateOrUpdateCart(int userId, string sessiontoken);
    Task GetProductCartAsync();
    Task UpdateCartAsync();
    Task DeleteProductCartAsync();
    Task AddProductOfCart(int userId, string sessiontoken, int productId, Role role);
}
