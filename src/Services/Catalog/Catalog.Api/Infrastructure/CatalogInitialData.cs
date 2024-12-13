using Catalog.Api.Domains;

namespace Catalog.Api.Infrastructure;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync())
        {
            return;
        }

        // Marten UPSERT will cater for existing records
        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync();

    }

    private IEnumerable<Product> GetPreconfiguredProducts() => new[]
    {
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone X",
            Description = "This phone is the company's biggest change to its",
            ImageFile = "product-1.png",
            Price = 950.00M,
            Quantity = 300,
            Categories = new List<string>()
            {
                "Smart Phone"
            }
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 10",
            Description = "The samsung flagship with amazing features",
            ImageFile = "Image-45.png",
            Categories = new List<string>()
            {
                "Smart Phone"
            },
            Price = 750.00M,
            Quantity = 230
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Hitachi Cooker",
            Description = "A cooker for rice and other receipes",
            ImageFile = "Image-52.png",
            Categories = new List<string>()
            {
                "Kitchen machine"
            },
            Price = 240.00M,
            Quantity = 43
        }
    };

}
