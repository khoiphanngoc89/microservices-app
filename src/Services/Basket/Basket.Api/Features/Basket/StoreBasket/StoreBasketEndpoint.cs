using Basket.Api.Entities;
using Basket.Api.Features.Basket.GetBaskets;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Features.Basket.StoreBasket;

public sealed record StoreBasketRequest(ShoppingCart Cart);
public sealed record StoreBasketResponse(string UserName);

public sealed class StoreBasketEndpoint
    : ICarterModule
{
    private const string Name = "StoreBasket";
    private const string Summary = "Store a basket";
    private const string Description = "Store a basket";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket",
            async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
