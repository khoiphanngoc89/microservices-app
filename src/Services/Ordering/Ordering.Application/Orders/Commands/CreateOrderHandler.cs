namespace Ordering.Application.Orders.Commands;

public sealed record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>;
public sealed record CreateOrderResult(Guid id);

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems is required");
    }
}

public class CreateOrderCommnadHandler(IOrderingDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {

        // create Order entity from command object
        // Save to db
        // return result
        var order = OrderTransformer.Init()
            .WithShippingAddress(command.Order.ShippingAddress)
            .WithBillingAddress(command.Order.BillingAddress)
            .WithPayment(command.Order.Payment)
            .Transform(command.Order);

        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }
}