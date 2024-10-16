﻿namespace App.Modules.FinanceTracking.Application.Transfers.GetTransfer;

public class TransferDto
{
    public required Guid Id { get; init; }

    public required Guid UserId { get; init; }

    public required Guid SourceWalletId { get; init; }

    public required Guid TargetWalletId { get; init; }

    public required DateTime MadeOn { get; init; }

    public required string Comment { get; init; }

    public required IReadOnlyCollection<string> Tags { get; init; }

    public required int SourceAmount { get; init; }

    public required string SourceCurrency { get; init; }

    public required int TargetAmount { get; init; }

    public required string TargetCurrency { get; init; }

    public required decimal ExchangeRate { get; init; }
}
