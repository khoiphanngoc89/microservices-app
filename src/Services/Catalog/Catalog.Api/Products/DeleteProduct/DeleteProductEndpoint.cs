namespace Catalog.Api.Products.DeleteProduct;

public sealed record DeleteProductResponse(bool IsSuccess);
public sealed class DeleteProductEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        });
    }
}
