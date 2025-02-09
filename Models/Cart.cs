namespace marketplace_api.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
}
