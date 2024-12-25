using Asp.Versioning;
using Ordering.Application.Orders.Queries;

namespace Ordering.Api.Endpoints;

// Accepts a name parameter.
// Constructs a GetOrderByNameQuery
// Returns a list of orders.

//public sealed record GetOrdersByNameRequest(string Name);
public sealed record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
public sealed class GetOrdersByName : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapGet("api/v{version:apiVersion}/orders/{name}", async (string name, ISender sender) =>
        {
            var results = await sender.Send(new GetOrderByNameQuery(name));
            var response = results.Adapt<GetOrdersByNameResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrdersByName")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get orders by order name")
        .WithDescription("Get orders by order name in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
