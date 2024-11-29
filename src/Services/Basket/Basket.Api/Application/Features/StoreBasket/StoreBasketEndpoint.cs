namespace Basket.Api.Application.Features.StoreBasket;

public sealed record StoreBasketRequest(ShoppingCart Cart);
public sealed record StoreBasketResponse(string UserName);

public sealed class StoreBasketEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/baskets", async (StoreBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("Store Basket")
        .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Store Basket")
        .WithDescription("Store Basket");
    }
}
