namespace Ordering.Domain.Abstractions;

public interface IEntityBase<T> : IAuditable
{
    public T Id { get; set; }
}