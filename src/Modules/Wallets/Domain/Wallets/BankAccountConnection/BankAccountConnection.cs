using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;

namespace App.Modules.Wallets.Domain.Wallets.BankAccountConnection;

public record BankAccountConnection(BankConnectionId Id, BankAccountId BankAccountId);
