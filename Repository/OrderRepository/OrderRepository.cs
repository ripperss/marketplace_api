using marketplace_api.CustomExeption;
using marketplace_api.Data;
using marketplace_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marketplace_api.Repository.OrderRepository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if(order == null)
        {
            throw new NotFoundExeption("данного заказа не существует");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        
    }

    public async Task<Order> GetOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(order => order.Products)
            .ThenInclude(pr => pr.Product)
            .FirstOrDefaultAsync(ord => ord.Id == orderId);
        if( order == null )
        {
            throw new NotFoundExeption("данный заказ не найден");
        }

        return order;
    }

    public async Task<List<Order>> GetOrdersAsync(int userid)
    {
        var orders =  _context.Orders
            .Include(ord => ord.Products)
            .ThenInclude(pr => pr.Product)
            .Where(order => order.UserId == userid);

        return await orders.ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByDate(DateTime date)
    {
        var startDate = date.Date;
        var endDate = startDate.AddDays(1).AddTicks(-1);

        return await _context.Orders
            .Where(o => o.DateCreated >= startDate && o.DateCreated <= endDate)
            .ToListAsync();
    }

    public async Task UpdateOfStatusAsync(OrderStatus orderStatus, int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new NotFoundExeption("данного заказа нет");
        }

        order.Status = orderStatus;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order newOrder, int orderId)
    {
        var order = await _context.Orders
            .Include(order => order.Products)
            .FirstOrDefaultAsync(ord => ord.Id == orderId);
        if(order == null)
        {
            throw new NotFoundExeption("Данный заказ не существует по этому ID");
        }

        order.DateCreated = newOrder.DateCreated;
        order.Status = newOrder.Status;
        order.Products = newOrder.Products;
        order.TotalPrice = newOrder.TotalPrice;

        await _context.SaveChangesAsync();
    }
}
