namespace App.Modules.Notifications.Application.Emails;

public class EmailMessage(
    IEnumerable<string> to,
    string subject,
    string content)
{
    public IEnumerable<string> To { get; } = to;

    public string Subject { get; } = subject;

    public string Content { get; } = content;

    public EmailMessage(
        string to,
        string subject,
        string content) : this(new[] { to }, subject, content)
    {
    }
}
