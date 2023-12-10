using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata;

public class PortfolioViewMetadata : Entity, IAggregateRoot
{
    public PortfolioId PortfolioId { get; }

    public string? Color { get; private set; }

    public string? Icon { get; private set; }

    public bool IsConsideredInTotalBalance { get; private set; }

    public static PortfolioViewMetadata CreateForPortfolio(PortfolioId portfolioId, string? color, string? icon, bool isConsideredInTotalBalance)
    {
        return new PortfolioViewMetadata(portfolioId, color, icon, isConsideredInTotalBalance);
    }

    private PortfolioViewMetadata(PortfolioId portfolioId, string? color, string? icon, bool isConsideredInTotalBalance)
    {
        PortfolioId = portfolioId;
        Color = color;
        Icon = icon;
        IsConsideredInTotalBalance = isConsideredInTotalBalance;
    }

    private PortfolioViewMetadata()
    { }
}
