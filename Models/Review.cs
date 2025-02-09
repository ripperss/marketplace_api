namespace marketplace_api.Models;

public class Review
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }  
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int ProductEvaluation { get; set; }
    public int Rating { get; set; }

}
