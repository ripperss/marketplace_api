using marketplace_api.Data;
using marketplace_api.Mapping;
using marketplace_api.Repository.UserRepository;
using marketplace_api.Services.AuthService;
using marketplace_api.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using marketplace_api.Extenions;

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


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Host=postgres-db;Port=5432;Username=postgres;Password=YourStrongPassword123!;Database=marketplace;");
});


builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddAuth(builder.Configuration);


var app = builder.Build();

app.UseAuthentication();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();


