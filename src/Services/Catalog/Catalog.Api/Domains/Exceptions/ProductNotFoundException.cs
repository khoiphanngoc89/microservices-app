using BuildingBlocks.Domains.Exceptions;

namespace Catalog.Api.Domains.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid Id)
        : base("Product", Id)
    {
    }
}
