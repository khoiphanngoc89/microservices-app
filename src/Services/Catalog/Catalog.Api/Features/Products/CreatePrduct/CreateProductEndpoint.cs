using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Api.Features.Products.CreatePrduct;

public sealed record CreateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
public sealed record CreateProductResponse(Guid Id);
public sealed class CreateProductEndpoint
    : ICarterModule
{
    private const string Name = "CreateProduct";
    private const string Summary = "Create a new product";
    private const string Description = "Create a new Product";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName(Name)
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
