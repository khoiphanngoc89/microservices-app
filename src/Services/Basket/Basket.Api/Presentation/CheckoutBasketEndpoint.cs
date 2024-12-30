using Basket.Api.Application.Basket;

namespace Basket.Api.Presentation;

public sealed record CheckoutBasketRequest(BasketCheckoutDto Basket);
public sealed record CheckoutBasketResponse(bool IsSuccess);

public sealed class CheckoutBasketEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/baskets/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CheckoutBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CheckoutBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Checkout a basket")
        .WithDescription("Checkout a basket"); ;
    }
}

