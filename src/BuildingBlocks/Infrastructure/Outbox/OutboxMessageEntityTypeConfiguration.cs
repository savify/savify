using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.Outbox;

public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    private readonly string _databaseSchemaName;

    public OutboxMessageEntityTypeConfiguration(string databaseSchemaName)
    {
        _databaseSchemaName = databaseSchemaName;
    }

    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", _databaseSchemaName);

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property<DateTime>("OccurredOn").HasColumnName("occurred_on");
        builder.Property<string>("Type").HasColumnName("type");
        builder.Property<string>("Data").HasColumnName("data");
        builder.Property<DateTime?>("ProcessedDate").HasColumnName("processed_date");
    }
}
