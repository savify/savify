using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

public class SaltEdgeCustomerEntityTypeConfiguration : IEntityTypeConfiguration<SaltEdgeCustomer>
{
    public void Configure(EntityTypeBuilder<SaltEdgeCustomer> builder)
    {
        builder.ToTable("salt_edge_customers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<Guid>("Identifier").HasColumnName("identifier");
    }
}
