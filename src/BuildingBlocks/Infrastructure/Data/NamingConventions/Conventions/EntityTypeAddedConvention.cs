using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class EntityTypeAddedConvention(INameRewriter nameRewriter) : IEntityTypeAddedConvention
{
    public void ProcessEntityTypeAdded(
        IConventionEntityTypeBuilder entityTypeBuilder,
        IConventionContext<IConventionEntityTypeBuilder> context)
    {
        var entityType = entityTypeBuilder.Metadata;

        if (entityType.BaseType is not null || entityType.ClrType.IsAbstract)
        {
            return;
        }

        if (entityType.GetTableName() is { } tableName)
        {
            entityTypeBuilder.ToTable(nameRewriter.RewriteName(tableName), entityType.GetSchema());
        }

        if (entityType.GetViewNameConfigurationSource() == ConfigurationSource.Convention
            && entityType.GetViewName() is { } viewName)
        {
            entityTypeBuilder.ToView(nameRewriter.RewriteName(viewName), entityType.GetViewSchema());
        }
    }
}
