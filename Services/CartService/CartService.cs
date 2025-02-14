using marketplace_api.Models;
using marketplace_api.Repository.CartRepository;
using marketplace_api.Services.ProductService;
using marketplace_api.CustomExeption;
using marketplace_api.Services.UserService;

namespace marketplace_api.Services.CartService;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task AddCartProductAsync(int productId, int userId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("индекс продукта не должен быть меньше нуля");
        }
        await _cartRepository.AddProductAsync(productId, userId);
    }

    public async Task<bool> CreateCartAsync(int userId)
    {
        
        var cart = new Cart()
        {
            UserId = userId
        };
        var success = await _cartRepository.CreateAsync(cart, userId);

        return success;

        
    }

    public async Task DeleteCartProductAsync(int productId, int userId)
    {
        await _cartRepository.DeleteProductAsync(userId, productId);
    }

    public async Task<List<CartProduct>> GetCarAlltProductsAsync(int userId)
    {
        var cartProduct = await _cartRepository.GetAllProductsAsync(userId);

        return cartProduct;
    }

    public async Task<Cart> GetCartAsync(int userId)
    {
        var cart = await _cartRepository.GetCartAsync(userId);

        return cart;
    }

    public async Task<CartProduct> GetCartProductAsync(int productId, int userId)
    {
        var cartProduct = await _cartRepository.GetProductoFCartAsync(userId, productId);

        return cartProduct;
    }

    public async Task<List<CartProduct>> GetProductCartPageAsync(int productId, int userId, int page)
    {
        if (page <= 0)
        {
            throw new Exception("номер страницы должен быть больше 0");
        }

        var cartProduct = await _cartRepository.GetGageProductAsync(userId, page);

        return cartProduct;
    }

    public async Task UpdateCartAsync(int userId, Cart newCart)
    {
        var cart = await _cartRepository.GetCartAsync(userId);

        await _cartRepository.UpdateAsync(newCart, userId);
    }
}