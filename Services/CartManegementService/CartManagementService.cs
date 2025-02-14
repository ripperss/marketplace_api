
using marketplace_api.Models;
using marketplace_api.Services.CartService;
using marketplace_api.Services.RedisService;

namespace marketplace_api.Services.CartManegementService;

public class CartManagementService : ICartManagementService
{
    private readonly ICartService _cartService;
    private readonly IRedisService _redisService;

    public CartManagementService(IRedisService redisService, ICartService cartService)
    {
        _redisService = redisService;   
        _cartService = cartService;
    }

    public async Task AddProductOfCart(int userId, string sessiontoken, int productId, Role role)
    {
        await _cartService.AddCartProductAsync(productId, userId);

        await _redisService.AddProductToCartAsync(sessiontoken, role, productId, 1); // по умолчанию добовляется всего один продукт в корзину
    }

    public async Task CreateOrUpdateCart(int userId ,string sessiontoken)
    {
        await _cartService.CreateCartAsync(userId);

        var cart = await _cartService.GetCartAsync(userId);
        var cartProducts = await _redisService.GetAllCartProductsAsync(sessiontoken);

        foreach (var product in cartProducts)
        {
            cart.CartProducts.Add(product);
        }

        await _cartService.UpdateCartAsync(userId, cart);
    }

    public Task DeleteProductCartAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetProductCartAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateCartAsync()
    {
        throw new NotImplementedException();
    }
}
