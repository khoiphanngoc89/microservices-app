namespace Catalog.Api.Features.Products.GetProductByCategory;

public sealed record GetProductByCategoryQuery(string Category)
    : IQuery<GetProductByCategoryResult>;
public sealed record GetProductByCategoryResult(IEnumerable<Product> Products);
public sealed class GetProductByCategoryQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .Where(p => p.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new(products);
    }
}
