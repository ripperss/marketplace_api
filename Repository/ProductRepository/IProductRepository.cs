using marketplace_api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace marketplace_api.Repository.ProductRepository;

public interface IProductRepository
{
    public Task<Product> GetByIdAsync(int id);
    public Task<List<Product>> GetAllAsync();
    public Task<Product> CreateAsync(Product product);
    public Task<Product> UpdateAsync(Product newProduct,int id);
    public Task DeleteAsync(int id);
    public Task<Product> PatchAsync(JsonPatchDocument<Product> productDto,int id);
    public Task<ICollection<Product>> GetByName(string name);
    public Task<List<Product>> GetProductOdPage(int id);
    
}
