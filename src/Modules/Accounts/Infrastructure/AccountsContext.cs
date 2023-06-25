using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure.Inbox;
using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;
using App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;
using App.Modules.Accounts.Infrastructure.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Infrastructure.Domain.Accounts.CreditAccounts;
using App.Modules.Accounts.Infrastructure.Domain.Accounts.DebitAccounts;
using App.Modules.Accounts.Infrastructure.Domain.Portfolios.InvestmentPortfolios;
using App.Modules.Accounts.Infrastructure.Inbox;
using App.Modules.Accounts.Infrastructure.InternalCommands;
using App.Modules.Accounts.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure;

public class AccountsContext : DbContext
{
    public DbSet<CashAccount>? CashAccounts { get; set; }

    public DbSet<CreditAccount>? CreditAccounts { get; set; }
    
    public DbSet<DebitAccount>? DebitAccounts { get; set; }

    public DbSet<InvestmentPortfolio>? InvestmentPortfolio { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }
    
    public DbSet<InboxMessage>? InboxMessages { get; set; }
    
    public DbSet<InternalCommand>? InternalCommands { get; set; }

    public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CashAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CreditAccountsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DebitAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InvestmentPortfolioEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
