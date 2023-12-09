using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios.Events;

namespace App.Modules.FinanceTracking.UnitTests.Portfolios;

[TestFixture]
public class InvestmentPortfoliosTests : UnitTestBase
{
    [Test]
    public void AddingInvestmentPortfolio_IsSuccessful()
    {
        var portfolio = InvestmentPortfolio.AddNew("InvestmentPortfolio");

        var portfolioAddedDomainEvent = AssertPublishedDomainEvent<InvestmentPortfolioAddedDomainEvent>(portfolio);

        Assert.That(portfolioAddedDomainEvent.PortfolioId, Is.EqualTo(portfolio.Id));
    }
}
