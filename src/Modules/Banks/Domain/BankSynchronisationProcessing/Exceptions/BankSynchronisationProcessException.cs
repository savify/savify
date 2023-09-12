using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BankSynchronisationProcessing.Exceptions;

public class BankSynchronisationProcessException : DomainException
{
    public BankSynchronisationProcessException(string? message) : base(message)
    {
    }
}
