using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class OrderProductDto
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }
    public ProductDto Product { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
