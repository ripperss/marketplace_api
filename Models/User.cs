namespace marketplace_api.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    public Role Role { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<ProductViewHistory> Products { get; set; } = new List<ProductViewHistory>();
}
