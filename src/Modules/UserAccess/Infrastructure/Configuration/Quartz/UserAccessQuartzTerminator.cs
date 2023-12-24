namespace App.Modules.UserAccess.Infrastructure.Configuration.Quartz;

public static class UserAccessQuartzTerminator
{
    public static void Terminate()
    {
        QuartzInitialization.StopQuartz();
    }
}
