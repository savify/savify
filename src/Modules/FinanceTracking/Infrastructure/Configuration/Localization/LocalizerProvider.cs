using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.FinanceTracking.Application.Configuration.Localization;
using Microsoft.Extensions.Localization;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Localization;

public class LocalizerProvider : ILocalizerProvider
{
    private readonly ILocalizerFactory _localizerFactory;

    private IStringLocalizer? _localizer;

    public LocalizerProvider(ILocalizerFactory localizerFactory)
    {
        _localizerFactory = localizerFactory;
    }

    public IStringLocalizer GetLocalizer()
    {
        if (_localizer is null)
        {
            CreateLocalizer();
        }

        return _localizer;
    }

    private void CreateLocalizer()
    {
        _localizer = _localizerFactory.Create<FinanceTrackingLocalizationResource>();
    }
}
