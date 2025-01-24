namespace Catalog.Api.Features.Products.GetProductByCategory;

public sealed record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint
    : ICarterModule
{
    private const string Name = "GetProductsByCategory";
    private const string Summary = "Get products by category from the system";
    private const string Description = "Get products by category from the system";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}",
            async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
