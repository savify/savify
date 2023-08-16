namespace App.BuildingBlocks.Infrastructure.Exceptions;

public class RepositoryException : Exception
{
    public override string Message => string.Format(MessageTemplate, MessageArguments);

    public string MessageTemplate;

    public object[] MessageArguments;

    public RepositoryException(string messageTemplate, Type objectType)
    {
        MessageTemplate = messageTemplate;
        MessageArguments = new object[] { objectType };
    }
}

public class RepositoryException<T> : RepositoryException
{
    public RepositoryException(string messageTemplate) : base(messageTemplate, typeof(T))
    {
        MessageTemplate = messageTemplate;
        MessageArguments = new object[] { typeof(T) };
    }
}
