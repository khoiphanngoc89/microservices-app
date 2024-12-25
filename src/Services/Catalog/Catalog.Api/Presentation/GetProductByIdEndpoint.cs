using Catalog.Api.Application.Catalog;

namespace Catalog.Api.Presentation;

public sealed record GetProductByIdResponse(Product Product);

public sealed class GetProductByIdEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id}", async (Guid id, ISender sender) =>
        {
            var entity = await sender.Send(new GetProductByIdQuery(id));
            var response = entity.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
