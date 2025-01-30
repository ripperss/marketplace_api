namespace marketplace_api.CustomExeption;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message):base(message) 
    { 
    }
}
