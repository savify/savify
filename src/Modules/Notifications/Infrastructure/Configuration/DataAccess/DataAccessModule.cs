using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration.DataAccess;

internal static class DataAccessModule
{
    internal static void Configure(IServiceCollection services, string databaseConnectionString)
    {
        services.AddScoped<ISqlConnectionFactory>(_ =>
        {
            return new SqlConnectionFactory(databaseConnectionString);
        });

        services.AddDbContext<NotificationsContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.AddScoped<DbContext, NotificationsContext>();

        services.Scan(scan =>
        {
            scan.FromAssemblies(Assemblies.Infrastructure)
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
    }
}
