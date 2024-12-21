namespace Ordering.Domain.Events;

public sealed record OrderCreatedEvent(Order order) : IDomainEvent;