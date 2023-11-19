using App.Modules.Transactions.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Modules.Transactions.Infrastructure.Domain.Finance;

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
            b.WithOwner().HasForeignKey("transaction_id");
            b.ToTable("transaction_sources", "transactions");

            b.OwnsOne(p => p.Sender, s =>
            {
                s.Property(o => o.Address).HasColumnName("source_sender_address");
            });

            b.OwnsOne(p => p.Amount, s => s.MoneyProperty("source_amount", "source_amount_currency"));
        });

        builder.OwnsOne<Target>("_target", b =>
        {
            b.WithOwner().HasForeignKey("transaction_id");
            b.ToTable("transaction_targets", "transactions");

            b.OwnsOne(p => p.Recipient, s =>
            {
                s.Property(o => o.Address).HasColumnName("target_recipient_address");
            });

            b.OwnsOne(p => p.Amount, s => s.MoneyProperty("target_amount", "target_amount_currency"));
        });

        builder.Property<DateTime>("_madeOn");

        builder.Property<string>("_comment").HasColumnName("comment");

        builder.Property<ICollection<string>>("_tags")
            .HasColumnName("tags")
            .HasConversion(
                a => string.Join(',', a),
                a => a.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasColumnType("text[]")
            .Metadata.SetValueComparer(
        new ValueComparer<ICollection<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToHashSet())); ;
    }
}
