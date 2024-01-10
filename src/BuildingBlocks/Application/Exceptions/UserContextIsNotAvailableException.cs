namespace App.BuildingBlocks.Application.Exceptions;

public class UserContextIsNotAvailableException(string? message) : ApplicationException(message);
