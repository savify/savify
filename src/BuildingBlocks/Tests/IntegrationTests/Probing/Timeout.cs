namespace App.BuildingBlocks.Tests.IntegrationTests.Probing;

public class Timeout(int duration)
{
    private readonly DateTime _endTime = DateTime.Now.AddMilliseconds(duration);

    public bool HasTimedOut()
    {
        return DateTime.Now > _endTime;
    }
}
