using marketplace_api.Models;

namespace marketplace_api.Repository.CartRepository;

public interface ICartRepository
{ 
    Task<bool> CreateAsync(Cart cart, int userId);
    Task AddProductAsync(int productId,int userId);
    Task UpdateAsync(Cart cart, int userId);
    Task DeleteAsync(int userId);
    Task<Cart> GetCartAsync(int userId);
    Task<CartProduct> GetProductoFCartAsync(int userId,int productId);
    Task<List<CartProduct>> GetGageProductAsync(int userId,int page);
    Task<List<CartProduct>> GetAllProductsAsync(int userId);
    Task DeleteProductAsync(int userId, int productId);

}
