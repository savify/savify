using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;

namespace App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;

public class Asset : Entity
{
    internal AssetId Id { get; private set; }

    private string _title;

    private decimal _amount;

    private string _tickerSymbol;

    private string _exchange;

    private string _country;

    private Money _purchasePrice;

    private DateTime? _purchasedAt;

    internal static Asset AddNew(string title, decimal amount, string tickerSymbol, string exchange, string country, Money purchasePrice, DateTime? purchasedAt)
    {
        return new Asset(title, amount, tickerSymbol, exchange, country, purchasePrice, purchasedAt);
    }

    private Asset(string title, decimal amount, string tickerSymbol, string exchange, string country, Money purchasePrice, DateTime? purchasedAt)
    {
        Id = new AssetId(Guid.NewGuid());
        _title = title;
        _amount = amount;
        _tickerSymbol = tickerSymbol;
        _exchange = exchange;
        _country = country;
        _purchasePrice = purchasePrice;
        _purchasedAt = purchasedAt;
    }

    private Asset()
    { }
}
