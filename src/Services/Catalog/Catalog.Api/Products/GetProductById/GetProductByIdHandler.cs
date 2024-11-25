namespace Catalog.Api.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public sealed record GetProductByIdResult(Product Product);

internal sealed class GetProductByIdQueryHandler (IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle Get Product by Id {@Id}", query.Id);
        var entity = await session.LoadAsync<Product>(query.Id, cancellationToken);
        if (entity is null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(entity);
    }
}
