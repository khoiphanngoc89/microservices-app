namespace Ordering.Domain.ValueObjects;

public sealed record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    public static OrderName Of(string value)
    {
        DomainException.ThrowIfNullOrWhitespace(value, nameof(OrderName));
        DomainException.ThrowIfGreaterThan(value.Length, DefaultLength, nameof(OrderName));
        return new OrderName(value);
    }
}
