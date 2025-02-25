using marketplace_api.CustomExeption;
using marketplace_api.Data;
using marketplace_api.Models;
using marketplace_api.ModelsDto;
using marketplace_api.Repository.OrderRepository;
using marketplace_api.Services.ProductService;

namespace marketplace_api.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductService _productService;
    private readonly AppDbContext _appDbContext;

    public OrderService(
        IOrderRepository orderRepository
        , IProductService productService
        , AppDbContext appDbContext)
    {
        _orderRepository = orderRepository;
        _productService = productService;
        _appDbContext = appDbContext;
    }

    public async Task CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        if(orderRequest == null)
        {
            throw new NotFoundExeption("Не заданны продукты которые должны попасть в заказ");
        }
        var order = new Order
        {
            UserId = orderRequest.UserId,
            DateCreated = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalPrice = 0,
            Products = new List<OrderProduct>()
        };

        foreach (var product in orderRequest.Products)
        {
            var productDb =  await _productService.GetProductAsync(product.ProductId);

            var orderProduct = new OrderProduct()
            {
                Price = productDb.Price,
                Quantity = product.Quantity,
                Product = productDb,
                ProductId = product.ProductId
            };

            order.Products.Add(orderProduct);
            order.TotalPrice += orderProduct.Price * orderProduct.Quantity;
        }

        using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        await _orderRepository.CreateOrderAsync(order);

        await transaction.CommitAsync();
    }// нужно также сделать так чтобы количество продуктов в бд уменьшалось 

    public Task DeleteOrderASync(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDto>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task OrderUpdateOfStatusAsync(OrderStatus orderStatus, int orderid)
    {
        throw new NotImplementedException();
    }
}
