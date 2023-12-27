namespace App.Modules.Notifications.Infrastructure.Configuration.Quartz;

public static class NotificationsQuartzTerminator
{
    public static void Terminate()
    {
        QuartzInitialization.StopQuartz();
    }
}
