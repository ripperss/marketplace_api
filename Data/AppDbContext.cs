using marketplace_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marketplace_api.Data;

public class AppDbContext : DbContext
{
    private IConfiguration _configuration;
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ProductViewHistory> ProductViewHistories { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options,IConfiguration configuration) : base(options)
    {
        Database.EnsureCreated();   
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Связь Review ↔ Product и User
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);

        // Связь OrderProduct (Многие-ко-многим)
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        // Связь CartProduct (Многие-ко-многим)
        modelBuilder.Entity<CartProduct>()
            .HasKey(cp => new { cp.CartId, cp.ProductId });

        base.OnModelCreating(modelBuilder);
    }
}
