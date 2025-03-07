using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Services.ProductService;

public interface IProductService
{
    Task<Product> GetProductAsync(int productId);
    Task<ICollection<Product>> GetAllProductsAsync();
    Task<Product> CreateProductAsync(Product product,int sellerId);
    Task<Product> UpdateProductAsync(Product product,int productId);
    Task DeleteProductAsync(int productId);
    Task<User> GetSellerProductsAsync(int productId);
    Task<Product> PatchProductAsync(JsonPatchDocument<Product> productPatch,int productId);
    Task<ICollection<Product>> GetProductByNameAsync(string name);
}
