using marketplace_api.CustomExeption;
using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marketplace_api.Repository.ProductViewHistoryRepository;

public class ProductViewHistoryRepository : IProductViewHistoryRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductViewHistoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddProducthistory(int userId, Product product)
    {

        var viewHistory = new ProductViewHistory
        {
            UserId = userId,
            ProductId = product.Id,
            Product = product
        };

        await _appDbContext.ProductViewHistories.AddAsync(viewHistory);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteProducthistory(int userId, int productId)
    {
        
        var viewHistory = await _appDbContext.ProductViewHistories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.ProductId == productId);

        if (viewHistory != null)
        {
            _appDbContext.ProductViewHistories.Remove(viewHistory);

            await _appDbContext.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundExeption("История просмотров не найдена");
        }
    }

    public async Task<IEnumerable<Product>> GetAllHistory(int userId)
    {
        var viewHistories = await _appDbContext.ProductViewHistories
        .Where(h => h.UserId == userId)
        .Include(h => h.Product) 
        .ToListAsync();
        if (viewHistories == null || !viewHistories.Any())
        {
            throw new NotFoundExeption("история покупок не нййдена у пользователя");
        }

        return viewHistories.Select(h => h.Product).ToList();
    }

    public async Task<Product> GetProduct(int userId, int productId)
    {
        var product = await _appDbContext.ProductViewHistories
            .FirstOrDefaultAsync(history => history.UserId == userId && history.ProductId == productId);

        if(product == null)
        {
            throw new NotFoundExeption("Данного продукта нет в истории");
        }

        return product.Product;
    }

    public async Task UpdateProducthistory(int userId, Product product,int productId)
    {
        var viewHistory = await _appDbContext.ProductViewHistories
            .FirstOrDefaultAsync(h => h.UserId==userId && h.ProductId == productId);
            
        if (viewHistory == null)
        {
            throw new NotFoundExeption("история покупок не нййдена у пользователя");
        }

        viewHistory.Product.Name = product.Name;
        viewHistory.Product.Description = product.Description;
        viewHistory.Product.Price = product.Price;

        await _appDbContext.SaveChangesAsync();
    }
}
