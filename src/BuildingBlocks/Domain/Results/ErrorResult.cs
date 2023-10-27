namespace App.BuildingBlocks.Domain.Results;

public class ErrorResult : Result
{
    public ErrorResult() : base(isSuccess: false)
    { }
}
