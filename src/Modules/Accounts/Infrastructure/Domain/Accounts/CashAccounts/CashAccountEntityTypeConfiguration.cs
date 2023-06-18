using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.CashAccounts;

internal class CashAccountEntityTypeConfiguration : IEntityTypeConfiguration<CashAccount>
{
    public void Configure(EntityTypeBuilder<CashAccount> builder)
    {
        builder.ToTable("cash_accounts", "accounts");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        
        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_balance").HasColumnName("balance");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(x => x.Value).HasColumnName("currency");
        });
    }
}
