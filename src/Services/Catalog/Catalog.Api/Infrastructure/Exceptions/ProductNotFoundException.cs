namespace Catalog.Api.Infrastructure.Exceptions;

public class ProductNotFoundException :Exception
{
    public ProductNotFoundException()
        : base("Product is not found")
    {
        
    }
}
