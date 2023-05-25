using Microsoft.Extensions.Localization;

namespace App.API.Configuration.Localization;

public interface ILocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create<TResource>() where TResource : ILocalizationResource, new();
}
