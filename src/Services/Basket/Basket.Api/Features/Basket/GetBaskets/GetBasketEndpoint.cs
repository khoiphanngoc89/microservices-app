using Basket.Api.Entities;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Features.Basket.GetBaskets;

public sealed record GetBasketResponse(ShoppingCart Cart);

public sealed class GetBasketEndpoint
    : ICarterModule
{
    private const string Name = "GetBasketByUserName";
    private const string Summary = "Get a basket by user name";
    private const string Description = "Get a basket by user name";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}",
            async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
