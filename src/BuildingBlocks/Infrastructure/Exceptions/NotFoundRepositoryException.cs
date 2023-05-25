namespace App.BuildingBlocks.Infrastructure.Exceptions;

public class NotFoundRepositoryException<T> : RepositoryException<T>
{
    private const string NotFoundMessageTemplate = "Object {0} with id '{1}' was not found";
    
    public NotFoundRepositoryException(Guid objectId) : base(NotFoundMessageTemplate, objectId)
    {
    }
}
