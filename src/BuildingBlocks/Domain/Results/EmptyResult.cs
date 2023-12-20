namespace App.BuildingBlocks.Domain.Results;

public class EmptyResult<TError>(TError? error) : Result(error is not null) where TError : class
{
    public new TError Error { get => error ?? throw new InvalidOperationException("Result is successful and does not have any error"); }

    public new bool IsSuccess => error is null;

    public new bool IsError => error is not null;

    public static implicit operator EmptyResult<TError>(TError error) => new(error);

    public static implicit operator EmptyResult<TError>(SuccessResult _) => new(error: null);
}
