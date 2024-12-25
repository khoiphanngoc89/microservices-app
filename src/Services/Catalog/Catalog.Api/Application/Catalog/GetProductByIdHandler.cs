using BuildingBlocks.Application.MediatR;

namespace Catalog.Api.Application.Catalog;

public sealed record GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public sealed record GetProductByIdResult(Product Product);

internal sealed class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await session.LoadAsync<Product>(query.Id, cancellationToken);
        ProductNotFoundException.ThrowIfNull(entity, query.Id.ToString());
        return new GetProductByIdResult(entity);
    }
}
