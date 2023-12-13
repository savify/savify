using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class ForeignKeyAddedConvention(INameRewriter nameRewriter) : ConversionBase(nameRewriter), IForeignKeyAddedConvention
{
    public void ProcessForeignKeyAdded(IConventionForeignKeyBuilder foreignKeyBuilder, IConventionContext<IConventionForeignKeyBuilder> context)
    {
        if (foreignKeyBuilder.Metadata.GetDefaultName() is { } constraintName)
        {
            foreignKeyBuilder.HasConstraintName(nameRewriter.RewriteName(constraintName));
        }
    }
}
