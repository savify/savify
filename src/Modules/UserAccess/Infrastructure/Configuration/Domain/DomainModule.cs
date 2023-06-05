using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Domain;

internal static class DomainModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<IUsersCounter, UsersCounter>();
        services.AddScoped<IUserDetailsProvider, UserDetailsProvider>();
    }
}
