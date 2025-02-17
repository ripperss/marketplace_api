using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class CartProductDto
{
    public ProductDto Product { get; set; }

    public int Quantity { get; set; }
}
