namespace Ordering.Domain.Entities;

public class Product : EntityBase<ProductId>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        return new Product
        {
            Id = id,
            Name = name,
            Price = price
        };
    }
}
