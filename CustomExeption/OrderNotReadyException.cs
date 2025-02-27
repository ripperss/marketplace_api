namespace marketplace_api.CustomExeption;

public class OrderNotReadyException : Exception
{
    public OrderNotReadyException(string message) : base(message)
    {

    }
}
