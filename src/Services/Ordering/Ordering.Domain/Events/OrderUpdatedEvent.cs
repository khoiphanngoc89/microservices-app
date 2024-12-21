namespace Ordering.Domain.Events;

public sealed record OrderUpdatedEvent(Order order) : IDomainEvent
{
}
