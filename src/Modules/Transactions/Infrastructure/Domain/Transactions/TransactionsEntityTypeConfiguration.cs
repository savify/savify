using App.Modules.Transactions.Domain.Transactions;
using App.Modules.Transactions.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Transactions.Infrastructure.Domain.Transactions;

internal class TransactionsEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions", "transactions");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

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
                s.Property(o => o.Address).HasColumnName("sender_address");
            });

            b.OwnsOne(p => p.Amount, s => s.MoneyProperty("amount", "amount_currency"));
        });

        builder.OwnsOne<Target>("_target", b =>
        {
            b.WithOwner().HasForeignKey("transaction_id");
            b.ToTable("transaction_targets", "transactions");

            b.OwnsOne(p => p.Recipient, s =>
            {
                s.Property(o => o.Address).HasColumnName("recipient_address");
            });

            b.OwnsOne(p => p.Amount, s => s.MoneyProperty("amount", "amount_currency"));
        });

        builder.Property<DateTime>("_madeOn").HasColumnName("made_on");

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
