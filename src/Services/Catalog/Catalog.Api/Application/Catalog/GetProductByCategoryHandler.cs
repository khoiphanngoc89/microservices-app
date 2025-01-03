﻿using BuildingBlocks.Application.MediatR;

namespace Catalog.Api.Application.Catalog;

public sealed record GetProductsByCategoryQuery(string Catagory)
    : IQuery<GetProductByCategoryResult>;
public sealed record GetProductByCategoryResult(IEnumerable<Product> Products);

internal sealed class GetProductByCatagoryQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var entities = await session.Query<Product>()
            .Where(x => x.Categories.Contains(query.Catagory))
            .ToListAsync(cancellationToken);
        return new GetProductByCategoryResult(entities);
    }
}
