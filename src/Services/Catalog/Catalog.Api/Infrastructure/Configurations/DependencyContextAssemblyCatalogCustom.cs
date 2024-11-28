using System.Reflection;

namespace Catalog.Api.Infrastructure.Configurations;

// By adding this class, the Carter will be scanned and got all its services in current assembly
// while Carter's nuget was installed in another assembly
public sealed class DependencyContextAssemblyCatalogCustom : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies() =>
    [
        typeof(Program).Assembly
    ];
}