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
using marketplace_api.Services.CartService;
using Hangfire.PostgreSql;
using Hangfire;
using marketplace_api;
using marketplace_api.Services.CartManegementService;
using marketplace_api.Repository.Rewiew;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomRoleActionFilter>();
    options.Filters.Add<CustomSessiontokenResousreFilter>();
});
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));


builder.Services.AddAutoMapper(typeof(UserProfiles),typeof(ProductProfiles), typeof(CartProductProfiles));


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

builder.Services.AddHangfire(h =>
    h.UsePostgreSqlStorage(connestString));
builder.Services.AddHangfireServer();

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
builder.Services.AddScoped<marketplace_api.Services.MailService>();
builder.Services.AddScoped<ICartManagementService, CartManagementService>();
builder.Services.AddScoped<IReviewRepository,ReviewRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/dash", new DashboardOptions
{
    Authorization = new[] { new AllowAllUsersAuthorizationFilter() }
});

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "market API"); 
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();


