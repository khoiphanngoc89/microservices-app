using Ordering.Application.Orders.Commands;

namespace Ordering.Api.Endpoints;

// Accepts a CreateOrderRequest  object
// Map the request to a CreateOrderCommand
// Use MediatR to send the command to corresponding handler
// Return a response with the created order's ID

public sealed record CreateOrderRequest(OrderDto Order);
public sealed record CreateOrderResponse(Guid OrderId);
public sealed class CreateOrder : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1.0))
        .ReportApiVersions()
        .Build();

        app.MapPost("api/v{version:apiVersion}/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateOrderResponse>();
            return Results.Created($"/orders/{response.OrderId}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new order")
        .WithDescription("Creates a new order in the system")
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1.0);
    }
}
