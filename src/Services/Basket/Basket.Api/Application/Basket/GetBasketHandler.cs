﻿using BuildingBlocks.Application.MediatR;

namespace Basket.Api.Application.Basket;

public sealed record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public sealed record GetBasketResult(ShoppingCart? Cart);

public sealed class GetBasketQueryHandler
    (IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(query.UserName, cancellationToken);
        return new GetBasketResult(basket);
    }
}