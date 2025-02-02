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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));


builder.Services.AddAutoMapper(typeof(UserProfiles));

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
builder.Services.AddScoped<IValidator<UserDto>,UserDtoValidator>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();


