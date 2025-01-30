using marketplace_api.Data;
using marketplace_api.Mapping;
using marketplace_api.Repository.UserRepository;
using marketplace_api.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(UserProfiles));

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) 
    .ReadFrom.Services(services) 
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Host=postgres-db;Port=5432;Username=postgres;Password=YourStrongPassword123!;Database=marketplace;");
});

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();


var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();


