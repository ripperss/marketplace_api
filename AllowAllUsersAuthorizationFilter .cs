using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace marketplace_api;

public class AllowAllUsersAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}

