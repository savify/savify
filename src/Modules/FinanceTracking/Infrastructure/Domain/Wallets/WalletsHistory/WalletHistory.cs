using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.Events;
using Dapper;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

public class WalletHistory
{
    public WalletId WalletId { get; private set; }

    public IList<WalletHistoryEvent> Events { get; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => Events.Select(e => e.ToDomainEvent()).AsList().AsReadOnly();

    public static WalletHistory From(Wallet wallet)
    {
        var walletHistory = new WalletHistory(wallet.Id, new List<WalletHistoryEvent>());

        foreach (var domainEvent in wallet.DomainEvents)
        {
            if (domainEvent is IWalletHistoryDomainEvent)
            {
                walletHistory.Events.Add(WalletHistoryEvent.From(domainEvent));
            }
        }

        return walletHistory;
    }

    public void AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent is IWalletHistoryDomainEvent)
            {
                Events.Add(WalletHistoryEvent.From(domainEvent));
            }
        }
    }

    private WalletHistory(WalletId walletId, IList<WalletHistoryEvent> events)
    {
        WalletId = walletId;
        Events = events;
    }

    private WalletHistory() { }
}
