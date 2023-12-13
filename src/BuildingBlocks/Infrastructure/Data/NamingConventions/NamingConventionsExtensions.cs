using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions;

public static class NamingConventionsExtensions
{
    public static DbContextOptionsBuilder UseSnakeCaseNamingConvention(this DbContextOptionsBuilder optionsBuilder, CultureInfo? culture = null)
    {
        var optionsExtension = optionsBuilder.Options.FindExtension<NamingConventionsOptionsExtension>() ??
                               new NamingConventionsOptionsExtension();
        var extension = optionsExtension.WithSnakeCaseNamingConvention(culture);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseSnakeCaseNamingConvention<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder, CultureInfo? culture = null) where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSnakeCaseNamingConvention((DbContextOptionsBuilder)optionsBuilder, culture);
}
