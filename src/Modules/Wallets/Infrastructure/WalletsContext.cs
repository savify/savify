using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.Wallets.Domain.Wallets.CashWallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;
using App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Infrastructure.Domain.BankConnections;
using App.Modules.Wallets.Infrastructure.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.Wallets.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.Wallets.Infrastructure.Domain.Wallets.CashWallets;
using App.Modules.Wallets.Infrastructure.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Infrastructure.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Infrastructure.Domain.Wallets.WalletViewMetadata;
using App.Modules.Wallets.Infrastructure.Inbox;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;
using App.Modules.Wallets.Infrastructure.InternalCommands;
using App.Modules.Wallets.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure;

public class WalletsContext : DbContext
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

    public WalletsContext(DbContextOptions<WalletsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CashWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CreditWalletsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DebitWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WalletViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InvestmentPortfolioEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PortfolioViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionProcessEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeCustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeConnectionEntityTypeConfiguration());
    }
}
