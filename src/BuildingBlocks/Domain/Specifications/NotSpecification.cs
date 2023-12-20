namespace App.BuildingBlocks.Domain.Specifications;

public class NotSpecification<T>(ISpecification<T> specification) : CompositeSpecification<T>
{
    public override bool IsSatisfiedBy(T t)
    {
        return !specification.IsSatisfiedBy(t);
    }
}
