using System.Reflection;

namespace Catalog.Api.Infrastructure.Configurations;

public class DependencyContextAssemblyCatalogCustom : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies() =>
    [
        typeof(Program).Assembly
    ];
}