﻿namespace Basket.Api.Application.Features.StoreBasket;

public sealed record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public sealed record StoreBasketResult(string UserName);

public sealed class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("User Name is required");
    }
}

public sealed class StoreBasketCommandHandler
    (IDocumentSession session)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        // store basket in database (use Master upsert - if exist = update. if not create new
        session.Store<ShoppingCart>(command.Cart);
        await session.SaveChangesAsync(cancellationToken);

        //TODO: update cache
        return new StoreBasketResult("swn");
    }
}
