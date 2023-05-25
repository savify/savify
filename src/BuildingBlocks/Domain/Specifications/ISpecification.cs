namespace App.BuildingBlocks.Domain.Specifications;

public interface ISpecification<T>
{
    public bool IsSatisfiedBy(T t);
}
