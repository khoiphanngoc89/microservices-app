namespace Catalog.Api.Domain;

public sealed class ProductNotFoundException
    : NotFoundException
{
    private const string Product = nameof(Product);
    public ProductNotFoundException(string Id)
    : base(Product, Id)
    {
    }

    public static void ThrowIfNull([NotNull] Product? product, [CallerArgumentExpression(nameof(product))] string paramName = default!)
    {
        if (product is null)
        {
            throw new ProductNotFoundException(paramName);
        }
    }
}
