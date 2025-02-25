namespace marketplace_api.ModelsDto;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public List<OrderProductRequest> Products { get; set; }
}
