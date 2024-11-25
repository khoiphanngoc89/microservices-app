
using Catalog.Api.Domains.Entities;

namespace Catalog.Api.Products.GetProducts;

public sealed record GetProductsResponse(IEnumerable<Product> Products);

public sealed class GetProductsEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var results = await sender.Send(new GetProductsQuery());
            var response = results.Adapt<GetProductsResponse>();
            return response;
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
