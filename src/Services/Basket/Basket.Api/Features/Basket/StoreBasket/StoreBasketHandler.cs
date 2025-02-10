using Basket.Api.Database;
using Basket.Api.Entities;
using BuildingBlocks.Common.Core;
using Discount.Grpc;

namespace Basket.Api.Features.Basket.StoreBasket;

public sealed record StoreBasketCommand(ShoppingCart Cart)
    : ICommand<StoreBasketResult>;
public sealed record StoreBasketResult(string UserName);

public sealed class StoreBasketCommandHandler
    (IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);

        // Store basket in database - (use Marten upsert - if exist = update, if not = create)
        var cart = await repository.StoreBasket(command.Cart, cancellationToken);
        return new (cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // communicate with Discount.Grpc and calculate lastest prices of product
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}