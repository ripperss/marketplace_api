using marketplace_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=market_place;Trusted_Connection=True");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();


