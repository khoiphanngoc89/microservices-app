using Basket.Api.Database;
using Basket.Api.Entities;
using BuildingBlocks.Common.Core;

namespace Basket.Api.Features.Basket.StoreBasket;

public sealed record StoreBasketCommand(ShoppingCart Cart)
    : ICommand<StoreBasketResult>;
public sealed record StoreBasketResult(string UserName);

public sealed class StoreBasketCommandHandler
    (IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var cart = await repository.StoreBasket(command.Cart, cancellationToken);
        return new (cart.UserName);
    }
}