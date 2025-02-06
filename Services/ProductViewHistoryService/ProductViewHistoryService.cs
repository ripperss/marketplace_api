using marketplace_api.Models;
using marketplace_api.Repository.ProductViewHistoryRepository;

namespace marketplace_api.Services.ProductViewHistoryService;

public class ProductViewHistoryService : IProductViewHistoryService
{
    private readonly IProductViewHistoryRepository _productViewHistoryRepository;

    public ProductViewHistoryService(IProductViewHistoryRepository productViewHistoryRepository)
    {
        _productViewHistoryRepository = productViewHistoryRepository;
    }

    public async Task AddHistoryAsync(Product product, int userId)
    {
        if (product == null)
        {
            throw new ArgumentNullException();
        }

        await _productViewHistoryRepository.AddProducthistory(userId, product);
    }

    public async Task DeleteHistoryAsync(int userId, int productId)
    {
        await _productViewHistoryRepository.DeleteProducthistory(userId,productId);
    }

    public async Task<IEnumerable<Product>> GetAllHistoryAsync(int userId)
    {
        var products = await _productViewHistoryRepository.GetAllHistory(userId);

        return products;
    }

    public async Task<Product> GetHistoryAsync(int userId, int productId)
    {
        var product = await _productViewHistoryRepository.GetProduct(userId,productId);

        return product;
    }

    public async Task UpdateHistoryAsync(Product product, int userId,int productId)
    {
        await _productViewHistoryRepository.UpdateProducthistory(userId,product,productId);
    }
}
