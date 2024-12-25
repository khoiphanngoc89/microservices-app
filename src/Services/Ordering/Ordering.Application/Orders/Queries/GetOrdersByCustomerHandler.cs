using BuildingBlocks.Application.MediatR;

namespace Ordering.Application.Orders.Queries;

public sealed record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;
public sealed record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);

public sealed class GetOrdersByCustomerQueryHandler(IOrderingDbContext dbContext)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        // get order by customer using dbContext
        // return result

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        var results = OrderDtoTransformer.Init()
            .Transform(orders);

        return new GetOrdersByCustomerResult(results);
    }
}
