using marketplace_api.Models;

namespace marketplace_api.Repository.ProductViewHistoryRepository;

public interface IProductViewHistoryRepository
{
    Task<IEnumerable<Product>> GetAllHistory(int userId);
    Task AddProducthistory(int userId,Product product);
    Task UpdateProducthistory(int userId,Product product,int productId);
    Task DeleteProducthistory(int userId,int productId);
    Task<Product> GetProduct(int userId,int productId);
}
