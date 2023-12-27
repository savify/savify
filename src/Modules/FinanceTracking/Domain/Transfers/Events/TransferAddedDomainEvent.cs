using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;
public class TransferAddedDomainEvent(TransferId transferId, WalletId sourceWalletId, WalletId targetWalletId, Money amount, CategoryId categoryId, DateTime madeOn, string comment, IEnumerable<string> tags) : DomainEventBase
{
    public TransferId TransferId { get; } = transferId;

    public WalletId SourceWalletId { get; } = sourceWalletId;

    public WalletId TargetWalletId { get; } = targetWalletId;

    public Money Amount { get; } = amount;


    public CategoryId CategoryId { get; } = categoryId;

    public DateTime MadeOn { get; } = madeOn;

    public string Comment { get; } = comment;

    public IEnumerable<string> Tags { get; } = tags;
}
