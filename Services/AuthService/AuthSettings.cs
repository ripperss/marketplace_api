namespace marketplace_api.Services.AuthService;

public class AuthSettings
{
    public TimeSpan Expires { get; set; }

    public string Secret { get; set; }
}
