﻿using App.Modules.Accounts.Application.AccountsViewMetadata;

namespace App.Modules.Accounts.Application.DebitAccounts.GetDebitAccount;

public class DebitAccountDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Currency { get; set; }
    public int Balance { get; set; }
    public AccountViewMetadataDto ViewMetadata { get; set; }
    public DateTime CreatedAt { get; set; }
}
