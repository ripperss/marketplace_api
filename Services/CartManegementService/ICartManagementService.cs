using marketplace_api.Models;
using Org.BouncyCastle.Bcpg;

namespace marketplace_api.Services.CartManegementService;

public interface ICartManagementService
{
    Task CreateOrUpdateCart(int userId, string sessiontoken);
    Task<CartProduct> GetProductCartAsync(HttpContext context, int productId);
    Task UpdateCartAsync(HttpContext context, Cart newCart);
    Task DeleteProductCartAsync(HttpContext context, int productId);
    Task AddProductOfCart(int productId, HttpContext context);
    Task<List<CartProduct>> GetPageProdcutOfCart(HttpContext context, int page);
    Task<List<CartProduct>> GetAllProductOfCartAsync(HttpContext context);
}
