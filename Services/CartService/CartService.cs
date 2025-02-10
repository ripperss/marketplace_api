using marketplace_api.Models;
using marketplace_api.Repository.CartRepository;
using marketplace_api.Services.RedisService;

namespace marketplace_api.Services.CartService;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IRedisService _redisService;

    public CartService(ICartRepository cartRepository, IRedisService redisService)
    {
        _cartRepository = cartRepository;
        _redisService = redisService;
    }

    public Task CreateCartProductAsync(Product product, int userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCartProductAsync(int productId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CartProduct>> GetCarAlltProductsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Cart> GetCartAsync(int productId, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartProduct> GetCartProductAsync(int productId, int userId)
    {
        var cartProduct = await _cartRepository.GetProductoFCartAsync(userId, productId);
        return cartProduct;
    }

    public Task<List<CartProduct>> GetProductCartPageAsync(int productId, int userId, int page)
    {
        throw new NotImplementedException();
    }
}
