using Basket.Api.Database;
using Basket.Api.Entities;
using BuildingBlocks.Common.Core;

namespace Basket.Api.Features.Basket.GetBaskets;

public sealed record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public sealed record GetBasketResult(ShoppingCart Cart);

public sealed class GetBasketQueryHandler
    (IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var cart = await repository.GetBasket(query.UserName);
        return new GetBasketResult(cart);
    }
}
