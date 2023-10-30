using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.InternalCommands;

public class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    private readonly string _databaseSchemaName;

    public InternalCommandEntityTypeConfiguration(string databaseSchemaName)
    {
        _databaseSchemaName = databaseSchemaName;
    }

    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("internal_commands", schema: _databaseSchemaName);

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property<string>("Type").HasColumnName("type");
        builder.Property<string>("Data").HasColumnName("data");
        builder.Property<DateTime>("EnqueueDate").HasColumnName("enqueue_date");
        builder.Property<DateTime?>("ProcessedDate").HasColumnName("processed_date");
        builder.Property<string?>("Error").HasColumnName("error");
    }
}
