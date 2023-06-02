namespace App.BuildingBlocks.Infrastructure.Exceptions;

public class RepositoryException : Exception
{
    public override string Message => string.Format(MessageTemplate, MessageArguments);

    public string MessageTemplate;
    
    public object[] MessageArguments;
    
    public RepositoryException(string messageTemplate, Type objectType, Guid objectId)
    {
        MessageTemplate = messageTemplate;
        MessageArguments = new object[] { objectType, objectId };
    }
}

public class RepositoryException<T> : RepositoryException
{
    public RepositoryException(string messageTemplate, Guid objectId) : base(messageTemplate, typeof(T), objectId)
    {
        MessageTemplate = messageTemplate;
        MessageArguments = new object[] { typeof(T), objectId };
    }
}
