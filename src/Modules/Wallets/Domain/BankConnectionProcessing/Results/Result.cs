using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Results;

public class Result<TSuccess, TError>
{
    private readonly TSuccess? _success;
    private readonly TError? _error;

    public TSuccess Success { get => _success ?? throw new InvalidOperationException(""); }
    public TError Error { get => _error ?? throw new InvalidOperationException(""); }

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

    public static implicit operator Result<TSuccess, TError>(TSuccess success) => new Result<TSuccess, TError>(success);
    public static implicit operator Result<TSuccess, TError>(TError error) => new Result<TSuccess, TError>(error);
}

public class Result<TSuccess> where TSuccess : class
{
    private readonly TSuccess? _success;

    public Result(TSuccess? success)
    {
        _success = success;
    }

    public TSuccess Success { get => _success ?? throw new InvalidOperationException(""); }

    public bool IsSuccess => _success is not null;


    public static implicit operator Result<TSuccess>(TSuccess success) => new(success);
    public static Result<TSuccess> Error() => new(success: null);
}
