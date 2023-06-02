namespace App.BuildingBlocks.Application.Emails;

public class EmailMessage
{
    public IEnumerable<string> To { get; }

    public string Subject { get; }

    public string Content { get; }

    public EmailMessage(
        IEnumerable<string> to,
        string subject,
        string content)
    {
        To = to;
        Subject = subject;
        Content = content;
    }
    
    public EmailMessage(
        string to,
        string subject,
        string content)
    {
        To = new []{to};
        Subject = subject;
        Content = content;
    }
}
