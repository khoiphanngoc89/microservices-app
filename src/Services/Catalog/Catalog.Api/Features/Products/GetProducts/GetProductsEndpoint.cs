namespace Catalog.Api.Features.Products.GetProducts;

public sealed record GetProductsResponse(IEnumerable<Product> Products);

public sealed class GetProductsEndpoint : ICarterModule
{
    private const string Name = "GetProducts";
    private const string Summary = "Get products";
    private const string Description = "Get products";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
            async (ISender sender) =>
            {
                var results = await sender.Send(new GetProductsQuery());
                var response = results.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
