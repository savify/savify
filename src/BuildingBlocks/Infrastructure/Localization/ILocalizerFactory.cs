using Microsoft.Extensions.Localization;

namespace App.BuildingBlocks.Infrastructure.Localization;

public interface ILocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create<TResource>() where TResource : ILocalizationResource, new();
}
