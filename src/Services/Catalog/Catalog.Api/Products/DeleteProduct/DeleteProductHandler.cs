
namespace Catalog.Api.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public sealed record DeleteProductResult(bool IsSuccess);

internal sealed class DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}
