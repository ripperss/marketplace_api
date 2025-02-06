using marketplace_api.Models;

namespace marketplace_api.Services.ProductViewHistoryService;

public interface IProductViewHistoryService
{
    Task<IEnumerable<Product>> GetAllHistoryAsync(int userId);
    Task<Product> GetHistoryAsync(int userId, int productId);
    Task AddHistoryAsync(Product product,int userId);
    Task UpdateHistoryAsync(Product product, int userId, int productId);
    Task DeleteHistoryAsync(int userId,int productId);
}
