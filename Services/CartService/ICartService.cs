using marketplace_api.Models;

namespace marketplace_api.Services.CartService;

public interface ICartService
{
    Task<List<CartProduct>> GetCarAlltProductsAsync(int userId);
    Task<CartProduct> GetCartProductAsync(int productId,int userId);
    Task<Cart> GetCartAsync(int productId,int userId);
    Task CreateCartProductAsync(Product product,int userId);
    Task DeleteCartProductAsync(int productId,int userId);
    Task<List<CartProduct>> GetProductCartPageAsync(int productId, int userId,int page);

}
