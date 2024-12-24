using BuildingBlocks.Application.Endpoints;

namespace Catalog.Api.Application.Features.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price,
    int Quantity);

public sealed record CreateProductResponse(Guid Id);

public sealed class CreateProductEndpoint : IEndpointModule
{
    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new product")
        .WithDescription("Create a new product");
    }
}