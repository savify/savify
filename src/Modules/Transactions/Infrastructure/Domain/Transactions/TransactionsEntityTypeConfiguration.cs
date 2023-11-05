using App.Modules.Transactions.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Transactions.Infrastructure.Domain.Transactions;

internal class TransactionsEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions", "transactions");

        builder.HasKey(t => t.Id);

        builder.Property<TransactionType>("_type")
            .HasColumnName("type")
            .HasConversion(
                a => a.Value,
                a => new TransactionType(a));

        builder.OwnsOne<Source>("_source", b =>
        {
            b.Property(p => p.Sender.Address).HasColumnName("sourceSenderAddress");
            b.Property(p => p.Amount.Amount).HasColumnName("sourceAmount");
            b.Property(p => p.Amount.Currency).HasColumnName("sourceAmmountCurrency");
        });

        builder.OwnsOne<Target>("_target", b =>
        {
            b.Property(p => p.Recipient.Address).HasColumnName("targetRecipientAddress");
            b.Property(p => p.Amount.Amount).HasColumnName("targetAmmount");
            b.Property(p => p.Amount.Currency).HasColumnName("targetCurrency");

        });

        builder.Property<DateTime>("_madeOn");

        builder.Property<string>("_comment").HasColumnName("comment");

        builder.Property<ICollection<string>>("_tags")
            .HasColumnName("tags")
            .HasConversion(
                v => v.ToArray(),
                v => v);
    }
}
