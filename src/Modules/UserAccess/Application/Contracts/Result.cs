namespace App.Modules.UserAccess.Application.Contracts;

public class Result
{
    public static Result Success => new(nameof(Success));

    private string _value;
    
    private Result(string value)
    {
        _value = value;
    }
}

public class Result<T>
{
    private T _value;

    private Result(T value)
    {
        _value = value;
    }
}
