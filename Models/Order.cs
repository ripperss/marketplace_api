using Microsoft.Extensions.Hosting;
using System.Data;

namespace marketplace_api.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<OrderProduct> Products { get; set; } = new List<OrderProduct>();

    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;



}
