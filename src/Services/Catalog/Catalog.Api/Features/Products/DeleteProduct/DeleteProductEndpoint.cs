namespace Catalog.Api.Features.Products.DeleteProduct;

public sealed record DeleteProductByCategoryResponse(bool IsSucceed);

public sealed class DeleteProductEndpoint
    : ICarterModule
{
    private const string Name = "DeleteProduct";
    private const string Summary = "Delete a product from the system";
    private const string Description = "Delete a product from the system";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}",
            async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                var response = result.Adapt<DeleteProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName(Name)
            .Produces<DeleteProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(Summary)
            .WithDescription(Description);
    }
}
