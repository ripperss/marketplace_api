using marketplace_api.Data;
using marketplace_api.Mapping;
using marketplace_api.Repository.UserRepository;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using marketplace_api.Extenions;
using FluentValidation;
using marketplace_api.ModelsDto;
using marketplace_api.CustomFilter;
using marketplace_api.Services.RedisService;
using marketplace_api.MappingProfiles;
using marketplace_api.Repository.ProductViewHistoryRepository;
using marketplace_api.Services.ProductViewHistoryService;
using marketplace_api.Repository.CartRepository;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using marketplace_api.Services.CartService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomRoleActionFilter>();
    options.Filters.Add<CustomSessiontokenResousreFilter>();
});
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));


builder.Services.AddAutoMapper(typeof(UserProfiles),typeof(ProductProfiles));


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) 
    .ReadFrom.Services(services) 
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

var connestString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis";
    options.InstanceName = "docker_network";
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connestString);
});


builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddScoped<IProductViewHistoryService,ProductViewHistoryService>();
builder.Services.AddScoped<IValidator<UserDto>,UserDtoValidator>();
builder.Services.AddScoped<IValidator<ProductDto>,ProductDtoValidator>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IProductViewHistoryRepository,ProductViewHistoryRepository>();
builder.Services.AddProd();
builder.Services.AddScoped<ICartRepository,CartRepository>();   
builder.Services.AddScoped<ICartService,CartService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();


