namespace Catalog.Api.Features.Products.CreatePrduct;

public sealed record CreateProductCommand(string Name,
                                          List<string> Categories,
                                          string Description,
                                          string ImageFile,
                                          decimal Price)
    : ICommand<CreateProductResult>;
public sealed record CreateProductResult(Guid Id);

public sealed class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    private const string NameErrorMessage = "Name is required";
    private const string CategoriesErrorMessage = "Categories is required";
    private const string ImageFileErrorMessage = "Image file is required";
    private const string PriceErrorMessage = "Price must be greater than 0";
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(NameErrorMessage);
        RuleFor(x => x.Categories).NotEmpty().WithMessage(CategoriesErrorMessage);
        RuleFor(x => x.ImageFile).NotNull().NotEmpty().WithMessage(ImageFileErrorMessage);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage(PriceErrorMessage);
    }
}    

public sealed class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        // save to database
        // return CreateProductResult result   

        Product product = new()
        {
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        return new (product.Id);
    }
}
