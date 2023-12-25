using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using App.Modules.FinanceTracking.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
public static class OwnsOneCurrencyExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsOneCurrency<TEntity>(this EntityTypeBuilder<TEntity> builder, string navigationName, string? currencyColumnName = null)
        where TEntity : class
    {
        return builder.OwnsOne<Currency>(navigationName, b =>
        {
            b.ConfigureCurrency(currencyColumnName);
        });
    }

    public static void OwnsOneCurrency<TEntity, TDependentEntity>(this OwnedNavigationBuilder<TEntity, TDependentEntity> builder, string navigationName, string? currencyColumnName = null)
        where TEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne<Currency>(navigationName, b =>
        {
            b.ConfigureCurrency(currencyColumnName);
        });
    }

    public static void OwnsOneCurrency<TEntity, TDependentEntity>(this OwnedNavigationBuilder<TEntity, TDependentEntity> builder, Expression<Func<TDependentEntity, Currency?>> navigationExpression, string? currencyColumnName = null)
        where TEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne(navigationExpression, b =>
        {
            b.ConfigureCurrency(currencyColumnName);
        });
    }

    private static void ConfigureCurrency<TEntity>(this OwnedNavigationBuilder<TEntity, Currency> builder, string? currencyColumnName = null)
        where TEntity : class
    {
        var valueProperty = builder.Property(p => p.Value);

        if (string.IsNullOrEmpty(currencyColumnName))
        {
            valueProperty.HasColumnName("currency");
        }
        else
        {
            valueProperty.HasColumnName(currencyColumnName);
        }
    }
}
