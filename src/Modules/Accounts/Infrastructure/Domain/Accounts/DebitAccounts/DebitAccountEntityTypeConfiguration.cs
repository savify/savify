using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;
using App.Modules.Accounts.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.DebitAccounts;

internal class DebitAccountEntityTypeConfiguration : IEntityTypeConfiguration<DebitAccount>
{
    public void Configure(EntityTypeBuilder<DebitAccount> builder)
    {
        builder.ToTable("debit_accounts", "accounts");

        builder.HasKey(account => account.Id);
        builder.Property(account => account.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_balance").HasColumnName("balance");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(currency => currency.Value).HasColumnName("currency");
        });
    }
}