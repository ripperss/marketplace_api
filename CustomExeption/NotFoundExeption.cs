namespace marketplace_api.CustomExeption;

public class NotFoundExeption : Exception
{
    public NotFoundExeption(string message) : base(message) { }
}
