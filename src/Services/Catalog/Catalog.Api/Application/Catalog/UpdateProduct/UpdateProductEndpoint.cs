namespace Catalog.Api.Application.Features.UpdateProduct;

public sealed record UpdateProductRequest(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price,
    int Quantity);

public sealed record UpdateProductResponse(bool IsSuccess);

public sealed class UpdateProductEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update a Product")
        .WithSummary("Update a Product");
    }
}
