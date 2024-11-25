
namespace Catalog.Api.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price,
    int Quantity)
    : ICommand<UpdateProductResult>;

public sealed record UpdateProductResult(bool IsSuccess);

internal sealed class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(product);
        product.Name = command.Name;
        product.Categories = command.Categories;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        product.Quantity = command.Quantity;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}
