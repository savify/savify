using App.Modules.Wallets.Domain.BankConnections;

namespace App.Modules.Wallets.Domain.Wallets;

public record BankAccountConnection(BankConnectionId Id, string BankAccountId);
