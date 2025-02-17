
using marketplace_api.Data;
using marketplace_api.Models;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.CartService;
using marketplace_api.Services.ProductService;
using marketplace_api.Services.RedisService;
using System.Diagnostics.Eventing.Reader;

namespace marketplace_api.Services.CartManegementService;

public class CartManagementService : ICartManagementService
{
    private readonly ICartService _cartService;
    private readonly IRedisService _redisService;
    private readonly JwtService _jwtService;
    private readonly AppDbContext _context;

    public CartManagementService(IRedisService redisService
        , ICartService cartService
        , JwtService jwtService
        , AppDbContext context)
    {
        _redisService = redisService;   
        _cartService = cartService;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task AddProductOfCart(int productId, HttpContext context)
    {
        Role role = Enum.Parse<Role>(context.Request.Cookies["role"] ?? "anonimus");
        var sessiontoken = context.Request.Cookies["sessionToken"];

        if (context.Request.Cookies.ContainsKey("token"))
        {
            var userId = _jwtService.GetIdUser(context);

            await _cartService.AddCartProductAsync(productId, userId);

            await _redisService.AddProductToCartAsync(sessiontoken, role, productId, 1); // по умолчанию добовляется всего один продукт в корзину

            return;
        }

        await _redisService.AddProductToCartAsync(sessiontoken, role, productId, 1);
    }

    public async Task CreateOrUpdateCart(int userId ,string sessiontoken)
    {
        await _cartService.CreateCartAsync(userId);

        var cart = await _cartService.GetCartAsync(userId);
        var cartProducts = await _redisService.GetAllCartProductsAsync(sessiontoken);

        foreach (var product in cartProducts)
        {
            var products = _context.Products.FirstOrDefault(pr => pr.Id == product.Product.Id);
            await _cartService.AddCartProductAsync(products.Id, userId);

            await _redisService.DeleteAllCartAsync(sessiontoken);
        }

        await _cartService.UpdateCartAsync(userId, cart);
    }

    public async Task DeleteProductCartAsync(HttpContext context, int productId)
    {
        var sessiontoken = context.Request.Cookies["sessionToken"];

        if (context.Request.Cookies.ContainsKey("token"))
        {
            var userId = _jwtService.GetIdUser(context);

            await _cartService.DeleteCartProductAsync(productId, userId);
            await _redisService.RemoveProductFromCartAsync(sessiontoken, productId);

            return;
        }

        await _redisService.RemoveProductFromCartAsync(sessiontoken, productId);
    }

    public async Task<List<CartProduct>> GetAllProductOfCartAsync(HttpContext context)
    {
        Role role = Enum.Parse<Role>(context.Request.Cookies["role"] ?? "anonimus");
        var sessiontoken = context.Request.Cookies["sessionToken"];

        if(context.Request.Cookies.ContainsKey("token"))
        {
            var userId = _jwtService.GetIdUser(context);

            var cartProducts = await _redisService.GetAllCartProductsAsync(sessiontoken);
            if(!cartProducts.Any())
            {
                var cartProductsDb = await _cartService.GetCarAlltProductsAsync(userId);

                foreach (var cartpr in cartProductsDb)
                {
                    await _redisService.AddProductToCartAsync(sessiontoken, role, cartpr.Product.Id, 1);
                }
                

                return cartProductsDb;
            }

            return cartProducts;
            
        }

        return await _redisService.GetAllCartProductsAsync(sessiontoken);
    }

    public async Task<List<CartProduct>> GetPageProdcutOfCart(HttpContext context, int page)
    {
        Role role = Enum.Parse<Role>(context.Request.Cookies["role"] ?? "anonimus");
        int pageSize = 10;
        var sessiontoken = context.Request.Cookies["sessionToken"];

        if(context.Request.Cookies.ContainsKey("token"))
        {
            var userId = _jwtService.GetIdUser(context);
            
            var cartProducts = await _redisService.GetPaginatedCartProductsAsync(sessiontoken, page, pageSize);

            if(!cartProducts.Any())
            {
                var cartProductsDb = await _cartService.GetProductCartPageAsync(userId, page);

                foreach(var cartPr in cartProductsDb)
                {
                    await _redisService.AddProductToCartAsync(sessiontoken, role, cartPr.Id, 1);
                }

                return cartProductsDb;
            }

            return cartProducts;
        }

        return await _redisService.GetPaginatedCartProductsAsync(sessiontoken, page, pageSize);
    }

    public async Task<CartProduct> GetProductCartAsync(HttpContext context, int productId)
    {
        Role role = Enum.Parse<Role>(context.Request.Cookies["role"] ?? "anonimus");
        var sessiontoken = context.Request.Cookies["sessionToken"];

        var cartProduct = await _redisService.GetProductFromCartAsync(sessiontoken, productId);
        if ( context.Request.Cookies.ContainsKey("token"))
        {
            try
            {
                return await _redisService.GetProductFromCartAsync(sessiontoken, productId);
            }

            catch (KeyNotFoundException ex)
            {
                int userId = _jwtService.GetIdUser(context);
                
                var cartProdcut = await _cartService.GetCartProductAsync(productId, userId);
                await _redisService.AddProductToCartAsync(sessiontoken, role, productId, 1);

                return cartProdcut;
            }
        }

        return  await _redisService.GetProductFromCartAsync(sessiontoken, productId);
    }

    public async Task UpdateCartAsync(HttpContext context, Cart newCart)
    {
        var userId = _jwtService.GetIdUser(context);
        var cart = await _cartService.GetCartAsync(userId);

        await _cartService.UpdateCartAsync(userId, newCart);
    }
}
