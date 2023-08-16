namespace App.BuildingBlocks.Application.Queries;

public struct PageData
{
    public int Offset { get; }

    public int Limit { get; }

    public PageData(int offset, int limit)
    {
        Offset = offset;
        Limit = limit;
    }
}
