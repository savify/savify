using App.Modules.FinanceTracking.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Finance;

internal static class OwnsOneMoneyExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsOneMoney<TEntity>(this EntityTypeBuilder<TEntity> builder, string navigationName, string? amountColumnName = null, string? currencyColumnName = null)
        where TEntity : class
    {
        builder.OwnsOne<Money>(navigationName, b =>
        {
            b.ConfigureMoney(amountColumnName, currencyColumnName);
        });

        builder.Navigation(navigationName).IsRequired();

        return builder;
    }

    public static void OwnsOneMoney<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, string navigationName, string? amountColumnName = null, string? currencyColumnName = null)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne<Money>(navigationName, b =>
        {
            b.ConfigureMoney(amountColumnName, currencyColumnName);
        });

        builder.Navigation(navigationName).IsRequired();
    }

    private static void ConfigureMoney<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, Money> builder, string? amountColumnName, string? currencyColumnName)
        where TOwnerEntity : class
    {
        var amountProperty = builder.Property(p => p.Amount).IsRequired();

        if (string.IsNullOrEmpty(amountColumnName))
        {
            amountProperty.HasColumnName("amount");
        }
        else
        {
            amountProperty.HasColumnName(amountColumnName);
        }

        builder.OwnsOneCurrency(p => p.Currency, currencyColumnName);
    }
}
