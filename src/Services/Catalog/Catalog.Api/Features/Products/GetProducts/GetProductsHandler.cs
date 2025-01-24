namespace Catalog.Api.Features.Products.GetProducts;

public sealed record GetProductsQuery()
    : IQuery<GetProductsResult>;
public sealed record GetProductsResult(IEnumerable<Product> Products);

public sealed class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new(products);
    }
}
