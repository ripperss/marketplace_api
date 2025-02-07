using marketplace_api.Models;
using marketplace_api.Repository.ProductViewHistoryRepository;
using marketplace_api.Services.ProductService;
using marketplace_api.Services.UserService;

namespace marketplace_api.Services.ProductViewHistoryService;

public class ProductViewHistoryService : IProductViewHistoryService
{
    private readonly IProductViewHistoryRepository _productViewHistoryRepository;
    private readonly IProductService _productService;

    public ProductViewHistoryService(
        IProductViewHistoryRepository productViewHistoryRepository
        , IProductService productService)
    {
        _productViewHistoryRepository = productViewHistoryRepository;
        _productService = productService;
    }

    public async Task AddHistoryAsync(Product product, int userId)
    {
        if (product == null)
        {
            throw new ArgumentNullException();
        }
        var productDb = await _productService.GetProductAsync(product.Id);

        await _productViewHistoryRepository.AddProducthistory(userId, productDb);
    }

    public async Task DeleteHistoryAsync(int userId, int productId)
    {
        var product = await _productService.GetProductAsync(productId); 
        
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
        var result = await _productService.GetProductAsync(productId);
        
        await _productViewHistoryRepository.UpdateProducthistory(userId,result,productId);
    }
}
