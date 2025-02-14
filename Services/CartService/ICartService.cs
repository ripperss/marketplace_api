using marketplace_api.Models;
using StackExchange.Redis;

namespace marketplace_api.Services.CartService;

public interface ICartService
{
    Task<List<CartProduct>> GetCarAlltProductsAsync(int userId);
    Task<CartProduct> GetCartProductAsync(int productId, int userId);
    Task<Cart> GetCartAsync(int userId);
    Task AddCartProductAsync(int productId, int userId);
    Task DeleteCartProductAsync(int productId, int userId);
    Task<List<CartProduct>> GetProductCartPageAsync(int productId, int userId, int page);
    Task<bool> CreateCartAsync(int userId);
    Task UpdateCartAsync(int userId, Cart newCart);


}