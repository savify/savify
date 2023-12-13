using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class PropertyAddedConvention(INameRewriter nameRewriter) : ConversionBase(nameRewriter), IPropertyAddedConvention
{
    public void ProcessPropertyAdded(
        IConventionPropertyBuilder propertyBuilder,
        IConventionContext<IConventionPropertyBuilder> context)
        => RewriteColumnName(propertyBuilder);
}
