using BuildingBlocks.Domains.Pagination;
using Ordering.Application.Orders.Queries;

namespace Ordering.Api.Endpoints;

// Accepts pagination parameters.
// Constructs a GetOrdersQuery with these parameters.
// Retrieves the data and returns it in a paginated format.
//public sealed record GetOrdersRequest(int Page, int PageSize);
public sealed record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public sealed class GetOrders : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapGet("api/v{version:apiVersion}/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var results = await sender.Send(new GetOrdersQuery(request));
            var response = results.Adapt<GetOrdersResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get pagination of orders")
        .WithDescription("Get pagination of orders in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
