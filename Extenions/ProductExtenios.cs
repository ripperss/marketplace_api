using marketplace_api.Repository.ProductRepository;
using marketplace_api.Services.ProductService;

namespace marketplace_api.Extenions;

public static class ProductExtenios
{
    public static IServiceCollection AddProd(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
