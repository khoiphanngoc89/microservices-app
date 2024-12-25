using Basket.Api.Application.Basket;

namespace Basket.Api.Presentation;

// public record GetBasketRequest(string UserName);
public sealed record GetBasketResponse(ShoppingCart Cart);

public sealed class GetBasketEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/baskets/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket By User Name")
        .WithDescription("Get Basket By User Name");
    }
}
