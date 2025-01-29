namespace marketplace_api.Models;

public class ProductPurchaseHistory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public DateTime DateOrder { get; set; } 
}
