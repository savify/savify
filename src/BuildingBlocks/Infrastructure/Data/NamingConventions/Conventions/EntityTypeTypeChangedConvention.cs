using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class EntityTypeTypeChangedConvention(INameRewriter nameRewriter) : IEntityTypeBaseTypeChangedConvention
{
    public void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder,
        IConventionEntityType? newBaseType, IConventionEntityType? oldBaseType, IConventionContext<IConventionEntityType> context)
    {
        var entityType = entityTypeBuilder.Metadata;

        if (newBaseType is null || entityType.GetMappingStrategy() == RelationalAnnotationNames.TpcMappingStrategy)
        {
            if (entityType.GetTableName() is { } tableName && !entityType.ClrType.IsAbstract)
            {
                entityTypeBuilder.ToTable(nameRewriter.RewriteName(tableName), entityType.GetSchema());
            }
        }
        else
        {
            entityTypeBuilder.HasNoAnnotation(RelationalAnnotationNames.TableName);
            entityTypeBuilder.HasNoAnnotation(RelationalAnnotationNames.Schema);
        }
    }
}
