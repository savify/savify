namespace App.Modules.Banks.Infrastructure.Configuration.Quartz;

public static class BanksQuartzTerminator
{
    public static void Terminate()
    {
        QuartzInitialization.StopQuartz();
    }
}
