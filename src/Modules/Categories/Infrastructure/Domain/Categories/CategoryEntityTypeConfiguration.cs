using App.Modules.Categories.Domain;
using App.Modules.Categories.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Categories.Infrastructure.Domain.Categories;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);

        builder.Property<string>(x => x.ExternalId);
        builder.Property<string>("_title");
        builder.Property<CategoryId?>("_parentId");

        builder.ComplexProperty<CategoryType>("_type", b =>
        {
            b.Property(x => x.Value).HasColumnName("type");
        });

        builder.OwnsOne<Url?>("_iconUrl", b =>
        {
            b.Property(x => x.Value).HasColumnName("icon_url");
        });
    }
}
