namespace marketplace_api.Models;

public class OrderProduct
{
    public int OrderId { get; set; } // FK для связи с Order
    public Order Order { get; set; } // Навигационное свойство

    public int ProductId { get; set; } // FK для связи с Product
    public Product Product { get; set; } // Навигационное свойство

    public int Quantity { get; set; } // Количество товара в заказе
    public decimal Price { get; set; }
}
