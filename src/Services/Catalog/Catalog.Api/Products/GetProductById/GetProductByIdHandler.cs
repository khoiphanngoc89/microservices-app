namespace Catalog.Api.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public sealed record GetProductByIdResult(Product Product);

internal sealed class GetProductByIdQueryHandler (IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await session.LoadAsync<Product>(query.Id, cancellationToken);
        if (entity is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        return new GetProductByIdResult(entity);
    }
}
