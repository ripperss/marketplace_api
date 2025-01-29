namespace marketplace_api.Models;

public class ProductViewHistory
{
    public int Id { get; set; } // Уникальный идентификатор записи

    public int UserId { get; set; } // FK для связи с User
    public User User { get; set; } // Навигационное свойство

    public int ProductId { get; set; } // FK для связи с Product
    public Product Product { get; set; } // Навигационное свойство

    public DateTime ViewDate { get; set; } = DateTime.UtcNow; // Дата просмотра
}
