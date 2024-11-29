namespace Basket.Api.Application.Features.DeleteBasket;

public sealed record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public sealed record DeleteBasketResult(bool IsSuccess);

public sealed class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("UserName cannot null or empty");
    }
}

public sealed class DeleteBasketCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // Delete basket by username
        session.DeleteWhere<ShoppingCart>(x => x.UserName.Equals(command.UserName));
        await session.SaveChangesAsync(cancellationToken);

        //TODO: Delete from cache

        return new DeleteBasketResult(true);
    }
}
