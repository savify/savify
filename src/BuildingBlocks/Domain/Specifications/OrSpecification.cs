namespace App.BuildingBlocks.Domain.Specifications;

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : CompositeSpecification<T>
{
    public override bool IsSatisfiedBy(T t)
    {
        return left.IsSatisfiedBy(t) || right.IsSatisfiedBy(t);
    }
}
