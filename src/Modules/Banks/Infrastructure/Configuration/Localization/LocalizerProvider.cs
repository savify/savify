using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Banks.Application.Configuration.Localization;
using Microsoft.Extensions.Localization;

namespace App.Modules.Banks.Infrastructure.Configuration.Localization;

public class LocalizerProvider(ILocalizerFactory localizerFactory) : ILocalizerProvider
{
    private IStringLocalizer? _localizer;

    public IStringLocalizer GetLocalizer()
    {
        if (_localizer is null)
        {
            CreateLocalizer();
        }

        return _localizer!;
    }

    private void CreateLocalizer()
    {
        _localizer = localizerFactory.Create<BanksLocalizationResource>();
    }
}
