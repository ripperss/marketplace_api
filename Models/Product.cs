using marketplace_api.ModelsDto;
using System.ComponentModel;

namespace marketplace_api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public Category Category { get; set; }
    public int UserId { get; set; } 
    public int CountProduct { get; set; }
    public int CountViewProduct { get; set; }


    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
