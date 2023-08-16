namespace App.BuildingBlocks.Domain.Specifications;

public class NotSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> _specification;

    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    public override bool IsSatisfiedBy(T t)
    {
        return !_specification.IsSatisfiedBy(t);
    }
}
