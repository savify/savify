using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using App.BuildingBlocks.Infrastructure.Data.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;

public static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection AddSqlConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(configuration.GetConnectionString("Savify")!));

        return services;
    }

    public static IServiceCollection AddDataAccessServices<TContext>(this IServiceCollection services, string databaseConnectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.UseSnakeCaseNamingConvention();
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
