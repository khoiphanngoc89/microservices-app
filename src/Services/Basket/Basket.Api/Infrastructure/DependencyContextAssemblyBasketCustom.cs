using System.Reflection;

namespace Basket.Api.Infrastructure;

public sealed class DependencyContextAssemblyBasketCustom
    : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies()
        => [typeof(Program).Assembly];
}
