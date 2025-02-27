using AutoMapper;
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
    private readonly IMapper _mapper; 

    public OrderService(
        IOrderRepository orderRepository
        , IProductService productService
        , AppDbContext appDbContext
        , IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productService = productService;
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            if (orderRequest == null)
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
                var productDb = await _productService.GetProductAsync(product.ProductId);

                var orderProduct = new OrderProduct()
                {
                    Price = productDb.Price,
                    Quantity = product.Quantity,
                    Product = productDb,
                    ProductId = product.ProductId
                };

                productDb.CountProduct -= orderProduct.Quantity;
                if (productDb.CountProduct <= 0)
                {
                    throw new Exception("Слишком много продуктов вы хотитие купить столько нет на складе");
                }

                await _productService.UpdateProductAsync(productDb, productDb.Id);

                order.Products.Add(orderProduct);
                order.TotalPrice += orderProduct.Price * orderProduct.Quantity;
            }

            await _orderRepository.CreateOrderAsync(order);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveOrderFromDeliveryAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderAsync(orderId);
        if (!(order.Status == OrderStatus.ready))
        {
            throw new OrderNotReadyException("Продукт еще не готов к выдачи и скорее всего в пути");
        }

        await _orderRepository.DeleteOrderAsync(orderId);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        if(orderId < 0)
        {
            throw new NotFoundExeption("заказа с таким айди не существует");
        }

        var order = await _orderRepository.GetOrderAsync(orderId);
        
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<List<OrderDto>> GetOrdersAsync(int userId)
    {
        var orders = await _orderRepository.GetOrdersAsync(userId);
        
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task OrderUpdateOfStatusAsync(OrderStatus orderStatus, int orderid)
    {
        await _orderRepository.UpdateOfStatusAsync(orderStatus, orderid);
    }

    public async Task<List<OrderDto>> GetOrdersByDate(DateTime date)
    {
        var orders = await _orderRepository.GetOrdersByDate(date);

        return _mapper.Map<List<OrderDto>>(orders.ToList());
    }

    public async Task UpdateOrderAsync(Order newOrder, int orderId)
    {
        await _orderRepository.UpdateOrderAsync(newOrder, orderId);
    }
}
