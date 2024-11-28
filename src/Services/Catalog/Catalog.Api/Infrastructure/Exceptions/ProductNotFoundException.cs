using BuildingBlocks.Cores.Exceptions;

namespace Catalog.Api.Infrastructure.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid Id)
        : base("Product", Id)
    {
    }
}
