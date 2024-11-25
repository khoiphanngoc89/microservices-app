
using Catalog.Api.Domains.Entities;

namespace Catalog.Api.Products.GetProducts;

public sealed record GetProductsQuery() : IQuery<GetProductsResult>;
public sealed record GetProductsResult(IEnumerable<Product> Products);
internal sealed class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}
