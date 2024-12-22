namespace Ordering.Domain.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; }
    private ProductId(Guid value) => Value = value;

    public static ProductId Of(Guid? value)
    {
        DomainException.ThrowIfNullOrEmpty(value, nameof(ProductId));
        return new ProductId(value.Value);
    }
}
