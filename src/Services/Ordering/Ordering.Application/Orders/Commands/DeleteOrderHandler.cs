using BuildingBlocks.Application.MediatR;

namespace Ordering.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid OrderId)
    : ICommand<DeleteOrderResult>;

public sealed record DeleteOrderResult(bool IsSucceed);

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
    }
}

public sealed class DeleteOrderCommandHandler(IOrderingDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var entity = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        OrderNotFoundException.ThrowIfNull(entity, orderId);
        dbContext.Orders.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(true);
    }
}
