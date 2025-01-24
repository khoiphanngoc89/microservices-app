namespace Catalog.Api.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id,
                                          string Name,
                                          List<string> Categories,
                                          string Description,
                                          string ImageFile,
                                          decimal Price)
    : ICommand<UpdateProductResult>;
public sealed record UpdateProductResult(bool IsSucceed);

public sealed class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    private const string ProductIdRequiredErrorMessage = "Product ID is required";
    private const string NameRequiredErrorMessage = "Name is required";
    private const string NameMaxLengthErrorMessage = "Name must be between 2 and 150 characters";
    private const string PriceErrorMessage = "Price must be greater than 0";

    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage(ProductIdRequiredErrorMessage);

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage(NameRequiredErrorMessage)
            .Length(2, 150).WithMessage(NameMaxLengthErrorMessage);

        RuleFor(command => command.Price)
            .GreaterThan(0).WithMessage(PriceErrorMessage);
    }
}

public sealed class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        ProductNotFoundException.ThrowIfNull(product, command.Id);
        product!.Name = product.Name;
        product.Categories = product.Categories;
        product.Description = product.Description;
        product.ImageFile = product.ImageFile;
        product.Price = product.Price;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new(true);
    }
}
