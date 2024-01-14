using System.Linq.Expressions;
using App.Modules.FinanceTracking.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Finance;

internal static class OwnsOneExchangeRateExtensions
{
    public static void OwnsOneExchangeRate<TEntity>(this EntityTypeBuilder<TEntity> builder, string navigationName,
        string? fromColumnName = null, string? toColumnName = null, string? rateColumnName = null)
        where TEntity : class
    {
        builder.OwnsOne<ExchangeRate>(navigationName, b =>
        {
            b.ConfigureExchangeRate(fromColumnName, toColumnName, rateColumnName);
        });

        builder.Navigation(navigationName).IsRequired();
    }

    public static void OwnsOneExchangeRate<TEntity, TDependentEntity>(this OwnedNavigationBuilder<TEntity, TDependentEntity> builder, Expression<Func<TDependentEntity, ExchangeRate?>> navigationExpression, string? fromColumnName = null, string? toColumnName = null, string? rateColumnName = null)
        where TEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne(navigationExpression, b =>
        {
            b.ConfigureExchangeRate(fromColumnName, toColumnName, rateColumnName);
        });

        builder.Navigation(navigationExpression).IsRequired();
    }

    public static void OwnsOneExchangeRate<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, string navigationName, string? fromColumnName = null, string? toColumnName = null, string? rateColumnName = null)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne<ExchangeRate>(navigationName, b =>
        {
            b.ConfigureExchangeRate(fromColumnName, toColumnName, rateColumnName);
        });

        builder.Navigation(navigationName).IsRequired();
    }

    private static void ConfigureExchangeRate<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, ExchangeRate> builder, string? fromColumnName, string? toColumnName, string? rateColumnName)
        where TOwnerEntity : class
    {
        builder.OwnsOneCurrency(p => p.From, fromColumnName);
        builder.OwnsOneCurrency(p => p.To, toColumnName);
        var rateProperty = builder.Property(p => p.Rate).IsRequired();

        if (string.IsNullOrEmpty(rateColumnName))
        {
            rateProperty.HasColumnName("rate");
        }
        else
        {
            rateProperty.HasColumnName(rateColumnName);
        }
    }
}
