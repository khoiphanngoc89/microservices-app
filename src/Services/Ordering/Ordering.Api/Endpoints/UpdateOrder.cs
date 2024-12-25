using Asp.Versioning;
using Ordering.Application.Orders.Commands;

namespace Ordering.Api.Endpoints;

// Accept a UpdateOrderRequest
// Map the request to an UpdateOrderCommand
// Send the command for processing
// Return a success or error response based on the outcome

public sealed record UpdateOrderRequest(OrderDto Order);
public sealed record UpdateOrderResponse(bool IsSuccess);
public sealed class UpdateOrder : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapPut("api/v{version:apiVersion}/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateOrderResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update a order")
        .WithDescription("Update a order in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
