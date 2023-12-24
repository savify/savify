namespace App.Modules.Categories.Infrastructure.Configuration.Quartz;

public static class CategoriesQuartzTerminator
{
    public static void Terminate()
    {
        QuartzInitialization.StopQuartz();
    }
}
