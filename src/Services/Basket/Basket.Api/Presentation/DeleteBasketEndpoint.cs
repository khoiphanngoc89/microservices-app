using Basket.Api.Application.Basket;

namespace Basket.Api.Presentation;

public sealed record DeleteBasketResponse(bool IsSuccess);

public sealed class DeleteBasketEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/baskets/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}