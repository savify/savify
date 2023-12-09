using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnections;
using App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure;

public class FinanceTrackingContext : DbContext
{
    public DbSet<CashWallet>? CashWallets { get; set; }

    public DbSet<CreditWallet>? CreditWallets { get; set; }

    public DbSet<DebitWallet>? DebitWallets { get; set; }

    public DbSet<WalletViewMetadata>? WalletsViewMetadata { get; set; }

    public DbSet<InvestmentPortfolio>? InvestmentPortfolios { get; set; }

    public DbSet<PortfolioViewMetadata> PortfoliosViewMetadata { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public DbSet<InboxMessage>? InboxMessages { get; set; }

    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public DbSet<BankConnectionProcess>? BankConnectionProcesses { get; set; }

    public DbSet<BankConnection>? BankConnections { get; set; }

    public DbSet<SaltEdgeCustomer>? SaltEdgeCustomers { get; set; }

    public DbSet<SaltEdgeConnection>? SaltEdgeConnections { get; set; }

    public FinanceTrackingContext(DbContextOptions<FinanceTrackingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new CashWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CreditWalletsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DebitWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WalletViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InvestmentPortfolioEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PortfolioViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionProcessEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeCustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeConnectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
