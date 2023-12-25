using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;
internal class TransfersRepository : ITransfersRepository
{
    private readonly FinanceTrackingContext _context;

    public TransfersRepository(FinanceTrackingContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transfer transfer)
    {
        await _context.AddAsync(transfer);
    }

    public void Remove(Transfer transfer)
    {
        _context.Remove(transfer);
    }
}

internal class TransferEntityTypeConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("transfers");

        builder.HasKey(t => t.Id);

        builder.Property<WalletId>("_sourceWalletId");
        builder.Property<WalletId>("_targetWalletId");
        builder.OwnsOneMoney("_amount", amountColumnName: "amount");
        builder.Property<DateTime>("_madeOn");
        builder.Property<string>("_comment");
        builder.PrimitiveCollection<IEnumerable<string>>("_tags");
    }
}
