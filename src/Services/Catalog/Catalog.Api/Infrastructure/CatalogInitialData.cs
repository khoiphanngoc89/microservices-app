namespace Catalog.Api.Infrastructure;

public sealed class CatalogInitialData : IInitialData
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
            Id = Guid.Parse("fc01856d-6660-4383-8bce-a8ab4f021c62"),
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
            Id = Guid.Parse("7b8d3b3e-7d24-4ae4-a9b4-05b482fe2bbf"),
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
            Id = Guid.Parse("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
            Name = "Huawei Plus",
            Description = "The product from China leader",
            ImageFile = "Image-52.png",
            Categories = new List<string>()
            {
                "Smart Phone"
            },
            Price = 650.00M,
            Quantity = 43
        },
        new Product()
        {
            Id = Guid.Parse("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
            Name = "Xiaomi Mi",
            Description = "The product of Apple China leader",
            ImageFile = "Image-52.png",
            Categories = new List<string>()
            {
                "Smart Phone"
            },
            Price = 650.00M,
            Quantity = 43
        }
    };

}
