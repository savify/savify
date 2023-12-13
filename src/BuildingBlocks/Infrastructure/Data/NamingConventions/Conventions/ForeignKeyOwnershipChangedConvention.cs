using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class ForeignKeyOwnershipChangedConvention(INameRewriter nameRewriter) : ConversionBase(nameRewriter), IForeignKeyOwnershipChangedConvention
{
    public void ProcessForeignKeyOwnershipChanged(IConventionForeignKeyBuilder relationshipBuilder, IConventionContext<bool?> context)
    {
        var foreignKey = relationshipBuilder.Metadata;
        var ownedEntityType = foreignKey.DeclaringEntityType;

        if (foreignKey.IsOwnership
            && (foreignKey.IsUnique || !string.IsNullOrEmpty(ownedEntityType.GetContainerColumnName()))
            && ownedEntityType.GetTableNameConfigurationSource() != ConfigurationSource.Explicit)
        {
            ownedEntityType.Builder.HasNoAnnotation(RelationalAnnotationNames.TableName);
            ownedEntityType.Builder.HasNoAnnotation(RelationalAnnotationNames.Schema);

            ownedEntityType.FindPrimaryKey()?.Builder.HasNoAnnotation(RelationalAnnotationNames.Name);

            foreach (var property in ownedEntityType.GetProperties())
            {
                RewriteColumnName(property.Builder);
            }
        }
    }
}
