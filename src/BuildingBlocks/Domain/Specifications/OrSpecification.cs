namespace App.BuildingBlocks.Domain.Specifications;

public class OrSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override bool IsSatisfiedBy(T t)
    {
        return _left.IsSatisfiedBy(t) || _right.IsSatisfiedBy(t);
    }
}
