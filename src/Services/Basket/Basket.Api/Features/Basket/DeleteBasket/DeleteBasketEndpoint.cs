using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Features.Basket.DeleteBasket;

public sealed record DeleteBasketResponse(bool IsSucceed);

public sealed class DeleteBasketEndpoint
    : ICarterModule
{
    private const string Name = "DeleteBasketByUserName";
    private const string Summary = "Delete a basket by user name";
    private const string Description = "Delete a basket by user name";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}",
            async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
