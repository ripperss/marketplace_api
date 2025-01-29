namespace marketplace_api.Models
{
    public class CartProduct
    {
        public int CartId { get; set; } // FK для связи с Cart
        public Cart Cart { get; set; } // Навигационное свойство

        public int ProductId { get; set; } // FK для связи с Product
        public Product Product { get; set; } // Навигационное свойство

        public int Quantity { get; set; } // Количество товара в корзине
    }
}
