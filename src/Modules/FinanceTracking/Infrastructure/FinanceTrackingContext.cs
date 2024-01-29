using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnections;
using App.Modules.FinanceTracking.Infrastructure.Domain.Budgets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Categories;
using App.Modules.FinanceTracking.Infrastructure.Domain.Expenses;
using App.Modules.FinanceTracking.Infrastructure.Domain.Incomes;
using App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;
using App.Modules.FinanceTracking.Infrastructure.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Infrastructure.Domain.Users.Tags;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletViewMetadata;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure;

public class FinanceTrackingContext(DbContextOptions<FinanceTrackingContext> options) : DbContext(options)
{
    public required DbSet<CashWallet> CashWallets { get; set; }

    public required DbSet<CreditWallet> CreditWallets { get; set; }

    public required DbSet<DebitWallet> DebitWallets { get; set; }

    public required DbSet<WalletViewMetadata> WalletsViewMetadata { get; set; }

    public required DbSet<WalletHistory> WalletHistories { get; set; }

    public required DbSet<InvestmentPortfolio> InvestmentPortfolios { get; set; }

    public required DbSet<PortfolioViewMetadata> PortfoliosViewMetadata { get; set; }

    public required DbSet<Category> Categories { get; set; }

    public required DbSet<Transfer> Transfers { get; set; }

    public required DbSet<Expense> Expenses { get; set; }

    public required DbSet<Income> Incomes { get; set; }

    public required DbSet<UserTags> UserTags { get; set; }

    public required DbSet<UserFinanceTrackingSettings> UserFinanceTrackingSettings { get; set; }

    public required DbSet<Budget> Budgets { get; set; }

    public required DbSet<OutboxMessage> OutboxMessages { get; set; }

    public required DbSet<InboxMessage> InboxMessages { get; set; }

    public required DbSet<InternalCommand> InternalCommands { get; set; }

    public required DbSet<BankConnectionProcess> BankConnectionProcesses { get; set; }

    public required DbSet<BankConnection> BankConnections { get; set; }

    public required DbSet<SaltEdgeCustomer> SaltEdgeCustomers { get; set; }

    public required DbSet<SaltEdgeConnection> SaltEdgeConnections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.Schema.Name);

        modelBuilder.ApplyConfiguration(new CashWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CreditWalletsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DebitWalletEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WalletViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WalletHistoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InvestmentPortfolioEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PortfolioViewMetadataEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionProcessEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankConnectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransferEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new IncomeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserTagsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserFinanceTrackingSettingsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeCustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SaltEdgeConnectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
