using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries;

public sealed record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;
public sealed record GetOrdersResult(PaginatedResult<OrderDto> Orders);

public sealed class GetOrdersQueryHandler(IOrderingDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageIndex,
                pageSize,
                totalCount,
                OrderDtoTransformer.Init().Transform(orders)));


    }
}
