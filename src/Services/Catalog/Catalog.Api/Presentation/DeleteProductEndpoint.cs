using Catalog.Api.Application.Catalog;

namespace Catalog.Api.Presentation;

public sealed record DeleteProductResponse(bool IsSuccess);
public sealed class DeleteProductEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProductById")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Product by Id")
        .WithDescription("Delete Product by Id");
    }
}
