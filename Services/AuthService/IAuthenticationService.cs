using marketplace_api.Models;

namespace marketplace_api.Services.AuthService
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUserAsync(User user);
        Task<string> LoginAsync(User user);
    }
}
