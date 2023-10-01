namespace App.BuildingBlocks.Domain.Results;

public class EmptyResult<TError> : Result
    where TError : class
{
    private readonly TError? _error;

    public TError Error { get => _error ?? throw new InvalidOperationException("Result is successful and does not have any error"); }

    public EmptyResult(TError? error) : base(error is not null)
    {
        _error = error;
    }

    public bool IsSuccess => _error is null;

    public bool IsError => _error is not null;

    public static implicit operator EmptyResult<TError>(TError error) => new(error);

    public static implicit operator EmptyResult<TError>(SuccessResult _) => new(error: null);
}
