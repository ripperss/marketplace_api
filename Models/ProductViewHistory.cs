namespace marketplace_api.Models;

public class ProductViewHistory
{
    public int Id { get; set; } 

    public int UserId { get; set; } 
    public User User { get; set; } 

    public int ProductId { get; set; } 
    public Product Product { get; set; } 

    public DateTime ViewDate { get; set; } = DateTime.UtcNow; 
}
