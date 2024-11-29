namespace Catalog.Api.Application.Features.GetProductsByCategory;

//public sealed record GetProductsByCatalogRequest
public sealed record GetProductsByCatalogryResponse(IEnumerable<Product> products);

public sealed class GetProductByCategoryEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/category/{categoryId}", async (string categoryId, ISender sender) =>
        {
            var results = await sender.Send(new GetProductsByCategoryQuery(categoryId));
            return Results.Ok(results.Adapt<GetProductsByCatalogryResponse>());
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductsByCatalogryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");
    }
}
