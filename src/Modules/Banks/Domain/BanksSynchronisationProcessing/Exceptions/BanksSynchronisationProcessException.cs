using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Exceptions;

public class BanksSynchronisationProcessException : DomainException
{
    public BanksSynchronisationProcessException(string? message) : base(message)
    {
    }
}
