using marketplace_api.Models;

namespace marketplace_api.Repository.OrderRepository;

public interface IOrderRepository
{
    Task<Order> GetOrderAsync(int orderId);
    Task<List<Order>> GetOrdersAsync(int userid);
    Task CreateOrderAsync(Order order); 
    Task DeleteOrderAsync(int orderId);
    Task UpdateOrderAsync(Order newOrder, int orderId);
    Task<List<Order>> GetOrdersByDate(DateTime date);
    Task UpdateOfStatusAsync(OrderStatus orderStatus, int orderid);
}

