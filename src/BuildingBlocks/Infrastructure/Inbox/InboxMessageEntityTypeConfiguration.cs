using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.Inbox;

public class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.OccurredOn);
        builder.Property<string>(m => m.Type);
        builder.Property<string>(m => m.Data);
        builder.Property(m => m.ProcessedDate);
    }
}
