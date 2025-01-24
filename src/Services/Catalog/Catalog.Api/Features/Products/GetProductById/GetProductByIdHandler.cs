namespace Catalog.Api.Features.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public sealed record GetProductByIdResult(Product Product);
public sealed class GetProductByIdQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        ProductNotFoundException.ThrowIfNull(product, query.Id);
        return new(product!);

    }
}
