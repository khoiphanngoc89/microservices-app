namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IAggregate, IEntityBase<T>
{
}

public interface IAggregate : IAuditable
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    IDomainEvent[] ClearDomainEvents();
}
