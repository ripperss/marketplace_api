using marketplace_api.Models;

namespace marketplace_api.Services.RedisService;

public interface IRedisService
{
    Task AddProductToCartAsync(string sessionToken, Role role, int productId, int quantity);

    Task<Cart> GetCartProductsAsync(string sessionToken);

    Task<CartProduct> GetProductFromCartAsync(string sessionToken, int productId);

    Task RemoveProductFromCartAsync(string sessionToken, int productId);

    Task<List<CartProduct>> GetPaginatedCartProductsAsync(string sessionToken, int pageNumber, int pageSize);

    Task DeleteAllCartAsync(string sessionToken);

    Task<List<CartProduct>> GetAllCartProduct(string sessionToken);
}