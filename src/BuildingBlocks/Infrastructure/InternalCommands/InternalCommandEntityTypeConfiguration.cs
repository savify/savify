using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.InternalCommands;

public class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("internal_commands");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property<string>(c => c.Type);
        builder.Property<string>(c => c.Data);
        builder.Property(c => c.EnqueueDate);
        builder.Property(c => c.ProcessedDate);
        builder.Property<string?>(c => c.Error);
    }
}
