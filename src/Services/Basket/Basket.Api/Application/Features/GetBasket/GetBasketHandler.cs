using Marten;

namespace Basket.Api.Application.Features.GetBasket;

public sealed record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public sealed record GetBasketResult(ShoppingCart? Cart);

public sealed class GetBasketQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var result = await session.Query<ShoppingCart>()
            .Where(x => x.UserName.Equals(query.UserName))
            .SingleOrDefaultAsync(cancellationToken);

        return new GetBasketResult(result);
    }
}
