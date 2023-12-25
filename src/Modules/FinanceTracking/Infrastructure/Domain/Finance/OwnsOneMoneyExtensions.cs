using App.Modules.FinanceTracking.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Finance;

internal static class OwnsOneMoneyExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsOneMoney<TEntity>(this EntityTypeBuilder<TEntity> builder, string navigationName, string? amountColumnName = null, string? currencyColumnName = null)
        where TEntity : class
    {
        return builder.OwnsOne<Money>(navigationName, b =>
        {
            b.ConfigureMoney(amountColumnName, currencyColumnName);
        });
    }

    public static void OwnsOneMoney<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, string navigationName, string? amountColumnName = null, string? currencyColumnName = null)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne<Money>(navigationName, b =>
        {
            b.ConfigureMoney(amountColumnName, currencyColumnName);
        });
    }

    private static void ConfigureMoney<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, Money> builder, string? amountColumnName, string? currencyColumnName)
        where TOwnerEntity : class
    {
        var amountProperty = builder.Property(p => p.Amount);

        if (!string.IsNullOrEmpty(amountColumnName))
        {
            amountProperty.HasColumnName(amountColumnName);
        }

        builder.OwnsOneCurrency(p => p.Currency, currencyColumnName);
    }
}
