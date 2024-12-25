using BuildingBlocks.Application.MediatR;

namespace Ordering.Application.Orders.Commands;

public sealed record UpdateOrderCommand(OrderDto Order)
    : ICommand<UpdateOrderResult>;

public sealed record UpdateOrderResult(bool IsSuccess);

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("OrderId is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
    }
}

public sealed class UpdateOrderCommandHandler(IOrderingDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        // update Order entity from command object
        // Save to db
        // return result
        var orderId = OrderId.Of(command.Order.Id);
        var entity = await dbContext.Orders.FindAsync([orderId], cancellationToken);

        OrderNotFoundException.ThrowIfNull(entity, orderId);

        var order = OrderTransformer.Init()
            .From(entity)
            .DataSource(command.Order)
            .WithShippingAddress(command.Order.ShippingAddress)
            .WithBillingAddress(command.Order.BillingAddress)
            .WithPayment(command.Order.Payment)
            .Transform();

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }
}
