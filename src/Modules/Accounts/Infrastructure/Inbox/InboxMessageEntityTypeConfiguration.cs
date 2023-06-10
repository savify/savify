using App.BuildingBlocks.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Inbox;

public class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages", "accounts");
        
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property<DateTime>("OccurredOn").HasColumnName("occurred_on");
        builder.Property<string>("Type").HasColumnName("type");
        builder.Property<string>("Data").HasColumnName("data");
        builder.Property<DateTime?>("ProcessedDate").HasColumnName("processed_date");
    }
}
