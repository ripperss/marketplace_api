using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.Services.OrderService;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequest orderRequest);
    Task<List<OrderDto>> GetOrdersAsync();
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task DeleteOrderASync(int orderId);
    Task OrderUpdateOfStatusAsync(OrderStatus orderStatus, int orderid);
}
