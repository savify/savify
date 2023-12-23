using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Exceptions;

public class BanksSynchronisationProcessException(string? message) : DomainException(message);
