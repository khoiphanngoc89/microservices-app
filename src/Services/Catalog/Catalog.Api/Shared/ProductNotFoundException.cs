using BuildingBlocks.Common.Core.Exceptions;
using Catalog.Api.Entities;

namespace Catalog.Api.Shared;

public sealed class ProductNotFoundException(Guid id) : NotFoundException(Product, id)
{
    private const string Product = nameof(Product);

    public static void ThrowIfNull(Product? value, Guid id)
    {
        if (value is null)
        {
            throw new ProductNotFoundException(id);
        }
    }
}
