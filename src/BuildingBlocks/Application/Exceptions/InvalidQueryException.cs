namespace App.BuildingBlocks.Application.Exceptions;

public class InvalidQueryException(string? message) : Exception(message);
