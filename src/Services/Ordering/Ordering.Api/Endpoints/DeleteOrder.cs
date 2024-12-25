using Asp.Versioning;
using Ordering.Application.Orders.Commands;

namespace Ordering.Api.Endpoints;

// Accepts the OrderID as a parameter and deletes the order
// Constructs a DeleteOrderCommand
// Send the command to the mediator
// Returns a success or not found response
//public sealed record DeleteOrderRequest(Guid OrderId);
public sealed record DeleteOrderRequespose(bool IsSuccess);
public sealed class DeleteOrder : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("api/v{version:apiVersion}/orders/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(id));
            return Results.Ok(new DeleteOrderRequespose(result.IsSucceed));
        })
        .WithName("DeleteOrder")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete a order")
        .WithDescription("Delete a order in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
