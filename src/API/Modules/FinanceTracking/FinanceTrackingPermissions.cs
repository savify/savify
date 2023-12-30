using Microsoft.AspNetCore.Connections.Features;

namespace App.API.Modules.FinanceTracking;

public static class FinanceTrackingPermissions
{
    public const string ManageWallets = nameof(ManageWallets);

    public const string ManageTransfers = nameof(ManageTransfers);

    public const string ConnectBankAccountsToWallets = nameof(ConnectBankAccountsToWallets);
}
