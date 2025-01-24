namespace Catalog.Api.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public sealed record DeleteProductResult(bool IsSucceed);

public sealed class DeleteProductCommandValidator
    : AbstractValidator<DeleteProductCommand>
{
    private const string ProductIdErrorMessage = "Product Id is required";
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage(ProductIdErrorMessage);
    }
}

public class DeleteProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);
        return new (true);
    }
}
