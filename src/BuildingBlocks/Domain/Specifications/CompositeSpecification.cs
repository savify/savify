namespace App.BuildingBlocks.Domain.Specifications;

public abstract class CompositeSpecification<T> : ISpecification<T>
{
    public CompositeSpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }
    
    public CompositeSpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    public CompositeSpecification<T> Not(ISpecification<T> specification)
    {
        return new NotSpecification<T>(this);
    }

    public abstract bool IsSatisfiedBy(T t);
}
