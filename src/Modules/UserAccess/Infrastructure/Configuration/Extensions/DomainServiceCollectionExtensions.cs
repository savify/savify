using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Extensions;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersCounter, UsersCounter>();
        services.AddScoped<IUserDetailsProvider, UserDetailsProvider>();

        return services;
    }
}
