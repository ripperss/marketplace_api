using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class ProductDtoResponse
{
    public int CountProduct { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public string Name { get; set; }
    public string Description { get; set; }
    public string imagePath { get; set; }
}
