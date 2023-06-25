using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Portfolios.InvestmentPortfolios.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;
public class InvestmentPortfolio : Entity, IAggregateRoot
{
    public PortfolioId Id { get; }

    internal UserId UserId { get; }

    private string _title;

    //Do we still want to have currency on investment portfolio?
    //Maybe we could store something more abstract than money - like investment type and amount.
    private Currency _currency;

    private DateTime _createdAt;

    public static InvestmentPortfolio AddNew(UserId userId, string title, Currency currency)
    {
        return new InvestmentPortfolio(userId, title, currency);
    }

    private InvestmentPortfolio(UserId userId, string title, Currency currency)
    {
        Id = new PortfolioId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _currency = currency;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new InvestmentPortfolioCreatedDomainEvent(Id, UserId, _currency));
    }

    private InvestmentPortfolio()
    { }
}
