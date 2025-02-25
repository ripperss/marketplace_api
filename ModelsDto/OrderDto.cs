using marketplace_api.Models;

namespace marketplace_api.ModelsDto;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public ICollection<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
}
