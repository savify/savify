using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

namespace App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

public record BankAccountConnection(BankConnectionId Id, BankAccountId BankAccountId);
