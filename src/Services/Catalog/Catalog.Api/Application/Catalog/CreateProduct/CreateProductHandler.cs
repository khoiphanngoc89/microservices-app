using BuildingBlocks.Application;
using Catalog.Api.Domains;

namespace Catalog.Api.Application.Features.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price,
    int Quantity)
    : ICommand<CreateProductResult>;
public sealed record CreateProductResult(Guid Id);

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name)
             .NotEmpty().WithMessage("Name is required")
             .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

internal sealed class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand commnand, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        Product entity = new()
        {
            Id = Guid.NewGuid(),
            Name = commnand.Name,
            Description = commnand.Description,
            ImageFile = commnand.ImageFile,
            Price = commnand.Price,
            Quantity = commnand.Quantity,
            Categories = commnand.Categories
        };

        // save to db
        session.Store(entity);
        await session.SaveChangesAsync(cancellationToken);

        // return product result
        return new CreateProductResult(entity.Id);
    }
}