namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Quartz;

public static class FinanceTrackingQuartzTerminator
{
    public static void Terminate()
    {
        QuartzInitialization.StopQuartz();
    }
}
