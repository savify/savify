using App.BuildingBlocks.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.UserAccess.Infrastructure.Outbox;

public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", "user_access");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property<DateTime>("OccurredOn").HasColumnName("occurred_on");
        builder.Property<string>("Type").HasColumnName("type");
        builder.Property<string>("Data").HasColumnName("data");
        builder.Property<DateTime?>("ProcessedDate").HasColumnName("processed_date");
    }
}
