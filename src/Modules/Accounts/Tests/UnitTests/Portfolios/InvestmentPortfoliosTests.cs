using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;
using App.Modules.Accounts.Domain.Portfolios.InvestmentPortfolios.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.UnitTests.Portfolios;

[TestFixture]
public class InvestmentPortfoliosTests : UnitTestBase
{
    [Test]
    public void AddingInvestmentPorfolio_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var portfolio = InvestmentPortfolio.AddNew(userId, "Investmnets", Currency.From("PLN"));

        var portfolioAddedDomainEvent = AssertPublishedDomainEvent<InvestmentPortfolioCreatedDomainEvent>(portfolio);

        Assert.That(portfolioAddedDomainEvent.PortfolioId, Is.EqualTo(portfolio.Id));
        Assert.That(portfolioAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(portfolioAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
