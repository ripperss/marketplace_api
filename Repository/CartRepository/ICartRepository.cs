using marketplace_api.Models;

namespace marketplace_api.Repository.CartRepository;

public interface ICartRepository
{ 
    Task CreateAsync(Cart cart, int userId);
    Task AddProductAsync(int productId,int userId);
    Task UpdateAsync(Cart cart, int userId);
    Task DeleteAsync(int userId);
    Task<Cart> GetCartAsync(int userId);
    Task<Product> GetProductoFCartAsync(int userId,int productId);
    Task<List<Product>> GetGageProductAsync(int userId,int page);
    Task<List<Product>> GetAllProductsAsync(int userId);


}
