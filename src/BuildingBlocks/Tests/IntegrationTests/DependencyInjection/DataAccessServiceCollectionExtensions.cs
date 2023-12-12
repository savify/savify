using App.BuildingBlocks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Tests.IntegrationTests.DependencyInjection;

public static class DataAccessServiceCollectionExtensions
{
    public static IServiceCollection ReplaceDbContext<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        var dbContextDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<TContext>));

        if (dbContextDescriptor is not null)
        {
            services.Remove(dbContextDescriptor);
        }

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        });

        return services;
    }
}
