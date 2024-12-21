using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public sealed record OrderItemId
{
    public Guid Value { get; }

    private OrderItemId(Guid value) => Value = value;

    public static OrderItemId Of(Guid? value)
    {
        DomainException.ThrowIfNullOrEmpty(value, nameof(OrderItemId));
        return new OrderItemId(value.Value);
    }
}
