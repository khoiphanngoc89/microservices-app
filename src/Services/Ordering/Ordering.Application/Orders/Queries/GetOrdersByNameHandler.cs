using BuildingBlocks.Application.MediatR;

namespace Ordering.Application.Orders.Queries;

public sealed record GetOrderByNameQuery(string OrderName) : IQuery<GetOrdersByNameResult>;
public sealed record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

public sealed class GetOrderByNameQueryValidator : AbstractValidator<GetOrderByNameQuery>
{
    public GetOrderByNameQueryValidator()
    {
        RuleFor(x => x.OrderName).NotNull().NotEmpty().WithMessage("OrderName is required");
    }
}
public sealed class GetOrdersByNameHandler(IOrderingDbContext dbContext)
    : IQueryHandler<GetOrderByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
    {
        // get orrder by name from db
        // return result
        var orders = await dbContext.Orders.Include(x => x.OrderItems)
            .AsNoTracking()
            .Where(x => x.OrderName.Value.Contains(query.OrderName))
            .OrderBy(x => x.OrderName)
            .ToListAsync(cancellationToken);

        var results = OrderDtoListTransformer.Init()
            .DataSource(orders)
            .Transform();
        return new GetOrdersByNameResult(results);

    }
}
