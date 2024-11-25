using Catalog.Api.Domains.Entities;

namespace Catalog.Api.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price,
    int Quantity)
    : ICommand<CreateProductResult>;
public sealed record CreateProductResult(Guid Id);

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