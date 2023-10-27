namespace App.BuildingBlocks.Domain.Results;

public class Result<TSuccess, TError>
{
    private readonly TSuccess? _success;

    private readonly TError? _error;

    public TSuccess Success { get => _success ?? throw new InvalidOperationException("Result is not successful"); }

    public TError Error { get => _error ?? throw new InvalidOperationException("Result is successful and does not have any error"); }

    public bool IsSuccess => _success is not null;

    public bool IsError => _error is not null;

    public Result(TSuccess success)
    {
        _success = success;
    }

    public Result(TError error)
    {
        _error = error;
    }

    public static implicit operator Result<TSuccess, TError>(TSuccess success) => new(success);

    public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
}

public class Result<TSuccess> where TSuccess : class
{
    private readonly TSuccess? _success;

    public Result(TSuccess? success)
    {
        _success = success;
    }

    public TSuccess Success { get => _success ?? throw new InvalidOperationException("Result is not successful"); }

    public bool IsSuccess => _success is not null;

    public bool IsError => _success is null;


    public static implicit operator Result<TSuccess>(TSuccess success) => new(success);

    public static implicit operator Result<TSuccess>(ErrorResult _) => new(success: null);

    public static Result<TSuccess> Error() => new(success: null);

}

public abstract class Result
{
    private readonly bool _isSuccess;

    public Result(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    public static SuccessResult Success => new();

    public static ErrorResult Error => new();

    public bool IsSuccess => _isSuccess;

    public bool IsError => !_isSuccess;
}
