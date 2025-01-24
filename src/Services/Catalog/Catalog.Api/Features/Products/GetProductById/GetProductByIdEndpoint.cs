namespace Catalog.Api.Features.Products.GetProductById;

public sealed record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint
    : ICarterModule
{
    private const string Name = "GetProductById";
    private const string Summary = "Get product by identifier from the system";
    private const string Description = "Get product by identifier from the system";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}",
            async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
