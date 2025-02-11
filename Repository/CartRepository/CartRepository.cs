using marketplace_api.CustomExeption;
using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace marketplace_api.Repository.CartRepository;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    
    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(int productId, int userId)
    {
        var cart = await GetUserCartAsync(userId);

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new NotFoundExeption("Product not found");

        var cartProduct = cart.CartProducts.FirstOrDefault(prd => prd.ProductId == productId);

        if (cartProduct == null)
        {
            cartProduct = new CartProduct()
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = 1,
                Product = product
            };
            cart.CartProducts.Add(cartProduct);
        }
        else
        {
            cartProduct.Quantity++;
        }

        await _context.SaveChangesAsync();

    }

    public async Task<bool> CreateAsync(Cart cart, int userId)
    {
        var cartDb = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cartDb != null)
        {
            return false;
        }

        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task DeleteAsync(int userId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if ( cart == null )
        {
            throw new NotFoundExeption("данный пользователь не найден");
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetCartAsync(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartProducts)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if( cart == null )
        {
            throw new NotFoundExeption($"Could not find {userId}");
        }

        return cart;
    }

    public async Task<List<CartProduct>> GetGageProductAsync(int userId, int page)
    {
        const int pageSize = 5;
        int skip = pageSize * (page - 1);

        var userCart = await GetUserCartAsync(userId);

        return userCart.CartProducts
            .Skip(skip)
            .Take(pageSize)
            .ToList();
    }

    public async Task<CartProduct> GetProductoFCartAsync(int userId, int productId)
    {
        var cart = await GetUserCartAsync(userId);

        var cartProduct = cart.CartProducts
            .FirstOrDefault(c => c.ProductId == productId);

        if (cartProduct == null)
            throw new NotFoundExeption("Product not found in cart");

        return cartProduct;
    }

    public async Task UpdateAsync(Cart cart, int userId)
    {
        var cartDb = await GetUserCartAsync(userId);

        cartDb.CartProducts = cart.CartProducts;
        await _context.SaveChangesAsync();
    }

    public async Task<List<CartProduct>> GetAllProductsAsync(int userId)
    {
        var userCart = await GetUserCartAsync(userId);
        return userCart.CartProducts.ToList();
    }

    private async Task<Cart> GetUserCartAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartProducts)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId) ??
            throw new NotFoundExeption("Корзина не создана");
    }

    public async Task DeleteProductAsync(int userId, int productId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId); if (cart == null)
        {
            throw new NotFoundExeption("данный пользователь не найден");
        }
        var product = await _context.Products.FirstOrDefaultAsync(pr => pr.Id == productId);
        if (product == null)
        {
            throw new NotFoundExeption("данный продукт не существует");
        }
        var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
        if (cartProduct == null)
        {
            throw new NotFoundExeption("Данного продукта нет в корзине");
        }
        cart.CartProducts.Remove(cartProduct);
        await _context.SaveChangesAsync();
    }
}