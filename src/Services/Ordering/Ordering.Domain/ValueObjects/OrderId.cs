namespace Ordering.Domain.ValueObjects;

public sealed record OrderId
{
    public Guid Value { get; }
    private OrderId(Guid value) => Value = value;
    public static OrderId Of(Guid? value)
    {
        DomainException.ThrowIfNullOrEmpty(value, nameof(OrderId));
        return new OrderId(value.Value);
    }
    public override string? ToString() => Value.ToString();
}
