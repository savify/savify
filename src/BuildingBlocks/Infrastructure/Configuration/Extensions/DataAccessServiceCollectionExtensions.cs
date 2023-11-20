using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration.Extensions;

public static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessServices<TContext>(this IServiceCollection services, string databaseConnectionString) where TContext : DbContext
    {
        services.AddScoped<ISqlConnectionFactory>(_ =>
        {
            return new SqlConnectionFactory(databaseConnectionString);
        });

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        services.Scan(scan =>
        {
            scan.FromAssemblies(typeof(TContext).Assembly)
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        return services;
    }
}
