using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions;

public static class NamingConventionsExtensions
{
    public static DbContextOptionsBuilder UseSnakeCaseNamingConvention(
        this DbContextOptionsBuilder optionsBuilder,
        CultureInfo? culture = null,
        bool ignoreMigrationsTable = true)
    {
        var optionsExtension = optionsBuilder.Options.FindExtension<NamingConventionsOptionsExtension>() ??
                               new NamingConventionsOptionsExtension();
        var extension = optionsExtension.WithSnakeCaseNamingConvention(culture, ignoreMigrationsTable);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseSnakeCaseNamingConvention<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        CultureInfo? culture = null,
        bool ignoreMigrationsTable = true) where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSnakeCaseNamingConvention((DbContextOptionsBuilder)optionsBuilder, culture, ignoreMigrationsTable);
}
