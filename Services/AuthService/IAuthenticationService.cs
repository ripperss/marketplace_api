using marketplace_api.Models;
using marketplace_api.ModelsDto;

namespace marketplace_api.Services.AuthService
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUserAsync(User user);
        Task<LoginResult> LoginAsync(User user, string sessiontoken);
    }
}
