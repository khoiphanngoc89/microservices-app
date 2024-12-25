using Asp.Versioning;
using Ordering.Application.Orders.Queries;

namespace Ordering.Api.Endpoints;

// Accepts a customer ID.
// Constructs a GetOrdersByCustomerQuery.
// Returns a list of orders for the customer.

//public sealed record GetOrderByCustomerRequest(string Customer);
public sealed record GetOrderByCustomerResponse(List<OrderDto> Orders);
public sealed class GetOrdersByCustomer : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
           .HasApiVersion(new ApiVersion(1.0))
           .ReportApiVersions()
           .Build();

        app.MapGet("api/v{version:apiVersion}/orders/customer/{id:Guid}", async (Guid id, ISender sender) =>
        {
            var results = await sender.Send(new GetOrdersByCustomerQuery(id));
            var response = results.Adapt<GetOrderByCustomerResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrdersByCustomer")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get orders by customer identifier")
        .WithDescription("Get orders by customer identifier in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
