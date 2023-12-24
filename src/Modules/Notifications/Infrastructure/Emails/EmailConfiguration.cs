namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailConfiguration
{
    public required string AppUrl { get; set; }

    public required string FromName { get; set; }

    public required string FromEmail { get; set; }

    public required string Host { get; set; }

    public int Port { get; set; }

    public bool UseSsl { get; set; }
}
