﻿using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;

internal class AddNewCreditWalletCommandHandler : ICommandHandler<AddNewCreditWalletCommand, Guid>
{
    private readonly ICreditWalletRepository _creditWalletRepository;
    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewCreditWalletCommandHandler(ICreditWalletRepository creditWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditWalletRepository = creditWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = CreditWallet.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.CreditLimit,
            command.AvailableBalance);

        await _creditWalletRepository.AddAsync(wallet);

        var viewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return wallet.Id.Value;
    }
}
