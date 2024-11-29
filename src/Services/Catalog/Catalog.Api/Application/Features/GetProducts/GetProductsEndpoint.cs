namespace Catalog.Api.Application.Features.GetProducts;
public sealed record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public sealed record GetProductsResponse(long PageSize, long PageNumber, long PageCount, IEnumerable<Product> Products);

public sealed class GetProductsEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var command = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(command);
            var response = new GetProductsResponse(result.Products.PageSize, result.Products.PageNumber, result.Products.PageCount, result.Products);
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
