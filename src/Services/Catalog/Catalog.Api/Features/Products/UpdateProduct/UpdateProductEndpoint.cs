namespace Catalog.Api.Features.Products.UpdateProduct;

public sealed record UpdateProductRequest(Guid Id,
                                          string Name,
                                          List<string> Categories,
                                          string Description,
                                          string ImageFile,
                                          decimal Price);
public sealed record UpdateProductResponse(bool IsSucceed);

public sealed class UpdateProductEndpoint
    : ICarterModule
{
    private const string Name = "UpdateProduct";
    private const string Summary = "Update a product in the system";
    private const string Description = "Update a product in the system";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
            async (UpdateProductRequest request, ISender sender) =>
            {
                var query = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(query);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
