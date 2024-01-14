using System.Globalization;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

namespace App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.CreateUserFinanceTrackingSettings;

internal class CreateUserFinanceTrackingSettingsCommandHandler(
    IUserFinanceTrackingSettingsRepository userFinanceTrackingSettingsRepository) : ICommandHandler<CreateUserFinanceTrackingSettingsCommand>
{
    public async Task Handle(CreateUserFinanceTrackingSettingsCommand command, CancellationToken cancellationToken)
    {
        var currency = GetCurrencyByCountryCode(command.CountryCode);

        var financeTrackingSettings = UserFinanceTrackingSettings.Create(
            new UserId(command.UserId),
            currency);

        await userFinanceTrackingSettingsRepository.AddAsync(financeTrackingSettings);
    }

    private static Currency GetCurrencyByCountryCode(string countryCode)
    {
        var cultureName = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .First(c => c.Name.EndsWith($"-{countryCode}"))
            .Name;

        var currency = Currency.From(new RegionInfo(cultureName).ISOCurrencySymbol);

        return currency;
    }
}
